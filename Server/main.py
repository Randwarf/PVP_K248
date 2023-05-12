from datetime import date, datetime, timedelta
import bcrypt
from flask import Flask, jsonify, request
from urllib.parse import unquote
from database import Database
from compare import calc_diff_percentages
import smtplib
from email.mime.text import MIMEText
from email.mime.image import MIMEImage
from email.mime.multipart import MIMEMultipart
import secrets
import datetime
import json
import datetime
app = Flask(__name__)
app.config.from_file("config.json", load=json.load)


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
                                  where=f"process={process}", 
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


@app.route("/get-main-app-history", methods=["GET"])
def get_main_app_history():
    db_result = [{}]
    args = request.args
    if "latest" in args.keys():
        db_result = database.select_data("apps", orderby="date DESC", limit="1")
    else:
        db_result = database.select_data("apps")
    db_result = jsonify(db_result)
    db_result.headers.add('Access-Control-Allow-Origin', '*')
    return db_result

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

    registered_user = database.select_data("users", where=f"email = {user_object['email']}")
    if len(registered_user) != 0:
        return jsonify({"code": "200", "message": "User with this email is already registered"}), 200

    password = user_object['password']
    password_bytes = password.encode('utf-8')
    salt = bcrypt.gensalt()
    hashed_password = bcrypt.hashpw(password_bytes, salt)
    user_object['password'] = hashed_password

    confirmation_token = secrets.token_urlsafe(32)
    current_time = datetime.datetime.now()
    expiration_time = current_time + datetime.timedelta(minutes=15)
    confirmation_token_object = {}
    confirmation_token_object['email'] = user_object['email']
    confirmation_token_object['token'] = confirmation_token
    confirmation_token_object['password'] = hashed_password
    confirmation_token_object['expiration_time'] = expiration_time

    existing_token = database.select_data("confirmationtokens", where=f"email = {args_json['email']}")
    if len(existing_token) != 0:
        database.update_data("confirmationtokens", confirmation_token_object, "email = ?", [confirmation_token_object['email']])
    else:
        database.insert_data("confirmationtokens", confirmation_token_object)

    confirmation_link = f"http://localhost:9000/signup-process.php?token={confirmation_token}"

    sender_email = 'pvpk248@gmail.com'
    sender_password = 'owkdkofqtqbfksvh'
    receiver_email = user_object['email']
    subject = 'eko-logika.lt svetainės prisiregistravimo patvirtinimas'
    message = f"""\
    <html>
      <body>
        <p>Jūsų elektroniniu paštu buvo bandoma registruotis eko-logika.lt puslapyje.</p>
        <br>
        <p>Norėdami patvirtinti savo paskyrą, prašome paspausti šią nuorodą: <a href="{confirmation_link}">Patvirtinti paskyrą</a></p>
        <p>Nuoroda galioja 15 minučių.</p>
        <br>
        <p>Jeigu registraciją inicijavote ne Jūs, šį laišką galite ignoruoti.</p>
        <p>eko-logika komanda</p>
        <img src="cid:image1" width="40" height="40">
      </body>
    </html>
    """
    msg = MIMEMultipart()
    msg['From'] = sender_email
    msg['To'] = receiver_email
    msg['Subject'] = subject
    msg.attach(MIMEText(message, 'html'))
    image_path = 'assets/images/logo.jpg'  # Replace with the actual path to your image file
    with open(image_path, 'rb') as image_file:
        image_attachment = MIMEImage(image_file.read())
        image_attachment.add_header('Content-ID', '<image1>')
        image_attachment.add_header('Content-Disposition', 'inline', filename='image.jpg')
    msg.attach(image_attachment)
    with smtplib.SMTP('smtp.gmail.com', 587) as server:
        server.starttls()
        server.login(sender_email, sender_password)
        server.sendmail(sender_email, receiver_email, msg.as_string())

    return jsonify({"code": "200", "message": "Patvirtinimo laiškas išsiųstas į nurodytą e-paštą."}), 200


@app.route("/create-user-confirmed", methods=["POST"])
def create_user_confirmed():
    content_type = request.headers.get('Content-Type')
    if "application/json" not in content_type:
        return jsonify({"code": "415", "message": "Unsupported media type"}), 415

    args_json = request.json
    user_object = {}

    existing_token = database.select_data("confirmationtokens", where=f"token = {args_json['token']}")
    if len(existing_token) == 0:
        return jsonify({"code": "401", "message": "Token does not exist"}), 200

    token = existing_token[0]
    current_time = datetime.datetime.now()
    expiration_time = datetime.datetime.strptime(token['expiration_time'], "%Y-%m-%d %H:%M:%S.%f")
    if current_time > expiration_time:
        database.delete_data("confirmationtokens", f"token = '{token['token']}'")
        return jsonify({"code": "402", "message": "Token has expired"}), 402

    user_object['email'] = token['email']
    user_object['password'] = token['password']

    database.delete_data("confirmationtokens", f"token = '{token['token']}'")
    database.insert_data("users", user_object)

    sender_email = 'pvpk248@gmail.com'
    sender_password = 'owkdkofqtqbfksvh'
    receiver_email = user_object['email']
    subject = 'eko-logika.lt svetainės sėkminga registracija'
    message = f"""\
    <html>
      <body>
        <p>Ačiū, jog prisijungėte prie mūsų svetainės ir domitės mūsų produktu!</p>
        <p>Jūsų paskyra buvo sėkmingai sukurta.</p>
        <p>Nuoširdžiai linkime, jog mūsų produktas padėtų Jums tausoti aplinką ir mažinti išnaudojamų resursų kiekį!</p>
        <p>eko-logika komanda</p>
        <img src="cid:image1" width="40" height="40">
      </body>
    </html>
    """
    msg = MIMEMultipart()
    msg['From'] = sender_email
    msg['To'] = receiver_email
    msg['Subject'] = subject
    msg.attach(MIMEText(message, 'html'))
    image_path = 'assets/images/logo.jpg'  # Replace with the actual path to your image file
    with open(image_path, 'rb') as image_file:
        image_attachment = MIMEImage(image_file.read())
        image_attachment.add_header('Content-ID', '<image1>')
        image_attachment.add_header('Content-Disposition', 'inline', filename='image.jpg')
    msg.attach(image_attachment)
    with smtplib.SMTP('smtp.gmail.com', 587) as server:
        server.starttls()
        server.login(sender_email, sender_password)
        server.sendmail(sender_email, receiver_email, msg.as_string())

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
        #registered_user['password'] = registered_password.decode('utf-8')
        Token = database.create_token(registered_user['id'], app.config['SECRET_KEY'])
        return jsonify(Token), 200

    return jsonify({"code": "200", "message": "Wrong password"}), 200

@app.route("/logout", methods=["POST"])
def logout():
    content_type = request.headers.get('Content-Type')
    if "application/json" not in content_type:
        return jsonify({"code": "415", "message": "Unsupported media type"}), 415

    args_json = request.json
    token = args_json['token']
    database.delete_data("AuthTokens", f"token='{token}'")
    return jsonify({"code": "200", "message": "Logged out"}), 200

@app.route("/get_user_by_token", methods=["POST"])
def get_user_by_token():
    content_type = request.headers.get('Content-Type')
    if "application/json" not in content_type:
        return jsonify({"code": "415", "message": "Unsupported media type"}), 415

    args_json = request.json
    token = args_json['token']
    tokenData = database.select_data("AuthTokens",where=f"token={token}")
    if len(tokenData) == 0:
        print("no token")
        return jsonify({"code": "404", "message": "Token not found"}), 404
    
    expiration_time_str = tokenData[0]['validBefore']
    expiration_time = datetime.datetime.fromtimestamp(expiration_time_str)
    if expiration_time < datetime.datetime.now():
        return jsonify({"code": "403", "message": "Token expired"}), 403

    userID = tokenData[0]['userID']
    user = database.select_data("Users", where=f"id={userID}")
    
    if len(user) == 0:
        print("no user")
        return jsonify({"code": "404", "message": "User not found"}), 404

    user = user[0]
    user['password'] = user['password'].decode('utf-8')
    return jsonify(user), 200

@app.route("/make_premium", methods=["GET"])
def make_premium():
    args = request.args
    
    if "email" not in args.keys():
        return jsonify({"code": "422", "message": "Not enough parameters - 'email'"}), 422
    
    buyers_email = args['email']
    default_premium_length = 30
    response = database.select_data('users', 'premiumEndDate', f'email = {buyers_email}')
    premium_end_date = response[0]['premiumEndDate']
    format_string = "%Y-%m-%d"
    date_obj = datetime.datetime.strptime(premium_end_date, format_string).date()

    if (date_obj >= date.today()):
        date_str = (date_obj + timedelta(days=default_premium_length)).strftime(format_string)
    else:
        date_str = (date.today() + timedelta(days=default_premium_length)).strftime(format_string)

    data = {
        'premiumEndDate': date_str
    }

    database.update_data('users', data, 'email=?', [buyers_email])

    return jsonify({"code": "200", "message": "Premium time added"}), 200

@app.route('/api_status', methods=['GET'])
def check_api_status():
    return '', 200

if __name__ == "__main__":
    app.run()