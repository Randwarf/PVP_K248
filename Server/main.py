from flask import Flask, jsonify, request
app = Flask(__name__)

database = []

@app.route("/save-benchmark", methods=["POST"])
def save_benchmark():
    content_type = request.headers.get('Content-Type')
    if "application/json" not in content_type:
        return jsonify({"code": "415", "message": "Unsupported media type"}), 415

    args_json = request.json
    required_args = ["cpu", "energy", "ram"]
    benchmark_object = {}

    for arg in required_args:
        if arg not in args_json:
            return jsonify({"code": "422", "message": "Not enough parameters"}), 422

        benchmark_object[arg] = args_json[arg]

    database.append(benchmark_object)
    return jsonify({"code": "200", "message": "Benchmark uploaded sucessfully"}), 200

@app.route("/get-benchmark", methods=["GET"])
def get_data():
    args = request.args
    
    if "id" in args.keys():
        id = int(args["id"])

        if id >= len(database):
            return jsonify({"code": "404", "message": "Resource with specified index was not found"})

        return jsonify(database[id])
    
    return jsonify(database)

if __name__ == "__main__":
    app.run()