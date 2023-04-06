import json
import bcrypt
from flask import Flask, jsonify, request
from urllib.parse import unquote
from database import Database
from compare import calc_diff_percentages
app = Flask(__name__)

database = Database("../Database/temp_db.db")
required_benchmark_args = ["date", "process", "cpu", "disk", "ram", "energy", "ip"]

@app.route("/save-benchmark", methods=["POST"])
def save_benchmark():
    content_type = request.headers.get('Content-Type')
    if "application/json" not in content_type:
        return jsonify({"code": "415", "message": "Unsupported media type"}), 415

    args_json = request.json
    benchmark_object = {}

    for arg in required_benchmark_args:
        if arg not in args_json:
            return jsonify({"code": "422", "message": "Not enough parameters"}), 422

        benchmark_object[arg] = args_json[arg]

    database.insert_data("benchmarks", benchmark_object)

    return jsonify({"code": "200", "message": "Benchmark uploaded sucessfully"}), 200

@app.route("/get-benchmark", methods=["GET"])
def get_benchmark():
    args = request.args
    
    if "id" in args.keys():
        id = int(args["id"])

        db_result = database.select_data("benchmarks", where=f"id = {id}")

        if db_result == None:
            return jsonify({"code": "404", "message": "Resource with specified index was not found"})

        return jsonify(db_result[0])
    
    db_result = database.select_data("benchmarks")

    return jsonify(db_result)

@app.route("/get-app", methods=["GET"])
def get_app():
    args = request.args

    if "process" in args.keys():
        process = args["process"]
        db_result = database.select_data("benchmarks",
                                  "process, COUNT(process) as count, round(AVG(cpu), 2) as cpu, round(AVG(disk), 2) as disk, round(AVG(ram), 2) as ram, round(AVG(energy), 2) as energy",
                                  where=f"process='{process}'", 
                                  groupby="process")

        if (len(db_result) == 0): # if the array is empty, the process is not in the database
            return jsonify({"code": "404", "message": "Process not found"}), 404

        db_result = jsonify(db_result)
        db_result.headers.add('Access-Control-Allow-Origin', '*')
        return db_result

    if "limit" not in args.keys():
        return jsonify({"code": "422", "message": "Not enough parameters - 'limit'"}), 422

    limit = int(args["limit"])
    db_result = database.select_data("benchmarks",
                                     "process, COUNT(process) as count, round(AVG(cpu), 2) as cpu, round(AVG(disk), 2) as disk, round(AVG(ram), 2) as ram, round(AVG(energy), 2) as energy",
                                          groupby="process",
                                          orderby="count DESC", 
                                          limit=limit)

    results = jsonify(db_result)
    results.headers.add('Access-Control-Allow-Origin', '*')

    return results

@app.route("/calc-diff", methods=["GET"])
def calc_diff():
    process1 = {}
    process2 = {}

    # Use request.args to get the query string parameters
    for key, value in request.args.items():
        # Check if the key belongs to process1 or process2
        if 'process1' in key:
            process1[key.split('[')[1][:-1]] = value
        elif 'process2' in key:
            process2[key.split('[')[1][:-1]] = value
    results = calc_diff_percentages(process1, process2)

    db_result = jsonify(results)
    db_result.headers.add('Access-Control-Allow-Origin', '*')
    return db_result

@app.route("/get-overall-stats", methods=["GET"])
def get_overall_stats():
    db_result = [{}]
    db_result = database.select_data("benchmarks", "COUNT(DISTINCT process) as apps_count, COUNT(process) as tests_count, COUNT(DISTINCT ip) as users_count")
    db_result = jsonify(db_result)
    db_result.headers.add('Access-Control-Allow-Origin', '*')
    return db_result

@app.route("/create-user", methods=["POST"])
def create_user():
    required_user_args = ["email", "password", "isPremium"]

    content_type = request.headers.get('Content-Type')
    if "application/json" not in content_type:
        return jsonify({"code": "415", "message": "Unsupported media type"}), 415

    args_json = request.json
    user_object = {}

    for arg in required_user_args:
        if arg not in args_json:
            return jsonify({"code": "422", "message": "Not enough parameters"}), 422
        
        user_object[arg] = args_json[arg]

    registered_user = database.select_data("users", where=f"email = {user_object['email']}")
    if len(registered_user) != 0:
        return jsonify({"code": "200", "message": "User with this email is already registered"}), 200

    password = user_object['password']
    password_bytes = password.encode('utf-8')
    salt = bcrypt.gensalt()
    hashed_password = bcrypt.hashpw(password_bytes, salt)
    user_object['password'] = hashed_password
    
    database.insert_data("users", user_object)

    return jsonify({"code": "200", "message": "User created sucessfully"}), 200

@app.route("/get-user-byid", methods=["GET"])
def get_user_byid():
    args = request.args
    
    if "id" not in args.keys():
        return jsonify({"code": "422", "message": "Not enough parameters - 'id'"}), 422

    id = int(args["id"])

    db_result = database.select_data("users", where=f"id = {id}")

    if len(db_result) == 0:
        return jsonify({"code": "404", "message": "Resource with specified id was not found"})

    user = db_result[0]
    user['password'] = user['password'].decode('utf-8')
    return jsonify(user)

@app.route("/get-user-byemail", methods=["GET"])
def get_user_byemail():
    args = request.args
    
    if "email" not in args.keys():
        return jsonify({"code": "422", "message": "Not enough parameters - 'email'"}), 422

    email = unquote(args["email"])

    db_result = database.select_data("users", where=f"email = {email}")

    if len(db_result) == 0:
        return jsonify({"code": "404", "message": "Resource with specified email was not found"})

    user = db_result[0]
    user['password'] = user['password'].decode('utf-8')
    return jsonify(user)

@app.route("/get-users", methods=["GET"])
def get_users():
    db_result = database.select_data("users")
    
    for result in db_result:
        result["password"] = result['password'].decode('utf-8')

    return jsonify(db_result)

@app.route("/login", methods=["POST"])
def login():
    required_user_args = ["email", "password"]

    content_type = request.headers.get('Content-Type')
    if "application/json" not in content_type:
        return jsonify({"code": "415", "message": "Unsupported media type"}), 415

    args_json = request.json
    user_object = {}

    for arg in required_user_args:
        if arg not in args_json:
            return jsonify({"code": "422", "message": "Not enough parameters"}), 422
        
        user_object[arg] = args_json[arg]

    registered_users = database.select_data("users", where=f"email = {user_object['email']}")
    if len(registered_users) == 0:
        return jsonify({"code": "200", "message": "User with this email is not registered"}), 200
    
    registered_user = registered_users[0]
    registered_password = registered_user['password']

    password = user_object['password']
    password_bytes = password.encode('utf-8')
    login_successful = bcrypt.checkpw(password_bytes, registered_password)

    if login_successful:
        registered_user['password'] = registered_password.decode('utf-8')
        return jsonify(registered_user), 200

    return jsonify({"code": "200", "message": "Wrong password"}), 200

if __name__ == "__main__":
    app.run()