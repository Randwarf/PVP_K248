from flask import Flask, jsonify, request
from database import Database
from compare import calc_diff_percentages
app = Flask(__name__)

database = Database("../Database/temp_db.db")
required_args = ["date", "process", "cpu", "disk", "ram", "energy"]

@app.route("/save-benchmark", methods=["POST"])
def save_benchmark():
    content_type = request.headers.get('Content-Type')
    if "application/json" not in content_type:
        return jsonify({"code": "415", "message": "Unsupported media type"}), 415

    args_json = request.json
    benchmark_object = {}

    for arg in required_args:
        if arg not in args_json:
            return jsonify({"code": "422", "message": "Not enough parameters"}), 422

        benchmark_object[arg] = args_json[arg]

    database.insert_data("benchmark", benchmark_object)

    return jsonify({"code": "200", "message": "Benchmark uploaded sucessfully"}), 200

@app.route("/get-benchmark", methods=["GET"])
def get_benchmark():
    args = request.args
    
    if "id" in args.keys():
        id = int(args["id"])

        db_result = database.select_data("benchmark", where=f"id = {id}")

        if db_result == None:
            return jsonify({"code": "404", "message": "Resource with specified index was not found"})

        return jsonify(db_result[0])
    
    db_result = database.select_data("benchmark")

    return jsonify(db_result)

@app.route("/get-app", methods=["GET"])
def get_app():
    args = request.args

    if "process" in args.keys():
        process = args["process"]
        db_result = database.select_data("benchmark",
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
    db_result = database.select_data("benchmark",
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

if __name__ == "__main__":
    app.run()