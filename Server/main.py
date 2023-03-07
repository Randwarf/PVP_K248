from flask import Flask, jsonify, request
from database import Database
app = Flask(__name__)

database = Database("../Database/temp_db.db")
required_args = ["process", "cpu", "disk", "ram", "energy"]

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
                                  "process, COUNT(process) as count, AVG(cpu) as cpu, AVG(disk) as disk, AVG(ram) as ram, AVG(energy) as energy", 
                                  where=f"process='{process}'", 
                                  groupby="process")
        
        db_result = jsonify(db_result)
        db_result.headers.add('Access-Control-Allow-Origin', '*')
        return db_result

    if "limit" not in args.keys():
        return jsonify({"code": "422", "message": "Not enough parameters - 'limit'"}), 422

    limit = int(args["limit"])
    db_result = database.select_data("benchmark",
                                          "process, COUNT(process) as count, AVG(cpu) as cpu, AVG(disk) as disk, AVG(ram) as ram, AVG(energy) as energy", 
                                          groupby="process",
                                          orderby="count DESC", 
                                          limit=limit)

    results = jsonify(db_result)
    results.headers.add('Access-Control-Allow-Origin', '*')

    return results

if __name__ == "__main__":
    app.run()