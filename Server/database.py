import sqlite3
import jwt #pip install PYjwt
import datetime

class Database:
    def __init__(self, path):
        self.connection = sqlite3.connect(path, check_same_thread=False)
        self.cursor = self.connection.cursor()
        self.cursor.row_factory = self.dict_factory

    def dict_factory(self, cursor, row):
        d = {}
        for idx, col in enumerate(cursor.description):
            d[col[0]] = row[idx]
        return d

    def create_table(self, table_name, columns):
        sql = f"CREATE TABLE {table_name} {columns}"
        self.cursor.execute(sql)
        self.connection.commit()
        print(f"Table {table_name} created successfully.")

    def insert_data(self, table_name, data):
        columns = ', '.join(data.keys())
        values = ', '.join(['?'] * len(data))
        sql = f"INSERT INTO {table_name} ({columns}) VALUES ({values})"
        self.cursor.execute(sql, tuple(data.values()))
        self.connection.commit()
        print(f"Data inserted successfully into {table_name}.")
        return self.cursor.lastrowid

    def select_data(self, table_name, columns='*', where=None, groupby=None, orderby=None, limit=None):
        cursor = self.new_cursor()
        sql = f"SELECT {columns} FROM {table_name}"
        params = []
        if where:
            parts = where.split('=')
            column = parts[0].strip()
            param = parts[1].strip()
            sql += f" WHERE {column} = ?"
            params.append(param)
        if groupby:
            sql += f" GROUP BY {groupby}"
        if orderby:
            sql += f" ORDER BY {orderby}"
        if limit:
            sql += f" LIMIT {limit}"
        cursor.execute(sql, params)
        rows = cursor.fetchall()
        cursor.close()
        return rows

    def update_data(self, table_name, data, condition, condition_values):
        set_values = ', '.join([f"{key} = ?" for key in data.keys()])
        sql = f"UPDATE {table_name} SET {set_values} WHERE {condition}"
        self.cursor.execute(sql, tuple(data.values()) + tuple(condition_values))
        self.connection.commit()
        print(f"{self.cursor.rowcount} rows updated successfully in {table_name}.")
        return self.cursor.rowcount

    def delete_data(self, table_name, where):
        sql = f"DELETE FROM {table_name} WHERE {where}"
        self.cursor.execute(sql)
        self.connection.commit()
        print(f"Data deleted successfully from {table_name}.")

    def create_token(self, userID, secretKey):
        expirationTime = datetime.datetime.utcnow() + datetime.timedelta(days=3)
        payload = {
            'userID': userID,
            'validBefore':expirationTime.timestamp()
        }
        token = jwt.encode(payload, secretKey, algorithm='HS256')

        payload["token"] = token
        self.insert_data("AuthTokens", payload)
        return token
    
    def close_connection(self):
        self.cursor.close()
        self.connection.close()
        print("Connection closed.")

    def new_cursor(self):
        cursor = self.connection.cursor()
        cursor.row_factory = self.dict_factory
        return cursor