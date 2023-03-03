from flask import Flask, jsonify, request
from itertools import groupby
app = Flask(__name__)

database = []
required_args = ["name","cpu", "energy", "ram"]

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

    database.append(benchmark_object)
    return jsonify({"code": "200", "message": "Benchmark uploaded sucessfully"}), 200

@app.route("/get-benchmark", methods=["GET"])
def get_benchmark():
    args = request.args
    
    if "id" in args.keys():
        id = int(args["id"])

        if id >= len(database):
            return jsonify({"code": "404", "message": "Resource with specified index was not found"})

        return jsonify(database[id])
    
    return jsonify(database)

@app.route("/get-app", methods=["GET"])
def get_app():
    args = request.args
    benchmarks = database

    if "name" in args.keys():
        name = args["name"]
        benchmarks = [b for b in benchmarks if b['name'] == name]
    
    if len(benchmarks) <= 0:
        return jsonify({"code": "404", "message": "Resource was not found"})

    allNames = {name for name in set(b['name'] for b in benchmarks)}
    param_sums = {name: {arg: 0 for arg in required_args[1:]} for name in allNames}
    param_counts = {name: {arg: 0 for arg in required_args[1:]} for name in allNames}
    for entry in benchmarks:
        for arg in required_args[1:]:
            param_sums[entry['name']][arg] += float(entry[arg])
            param_counts[entry['name']][arg] += 1

    results = []
    for name in allNames:
        entry = {'name' : name,
                 'count': param_counts[name][required_args[1]]}
        for arg in required_args[1:]:
            entry[arg] = param_sums[name][arg]/param_counts[name][arg]
        results.append(entry)

    results = sorted(results, key = lambda x: x['count'], reverse=True)

    if "limit" in args.keys():
        limit = int(args["limit"])
        results = results[0:limit]

    results = jsonify(results)
    results.headers.add('Access-Control-Allow-Origin', '*')

    return results

if __name__ == "__main__":
    app.run()