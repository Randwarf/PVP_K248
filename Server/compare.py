def calc_diff_percentages(process1, process2, threshold=10):
    difference = {}
    total_diff = 0

    for key, value in process1.items():
        if key == "cpu" or key == "ram" or key == "disk":
            percentage1 = float(value)
            percentage2 = float(process2[key])
            if percentage1 == percentage2:
                diff_percent = 0
            else:
                bigger_metric = max(percentage1, percentage2)
                smaller_metric = min(percentage1, percentage2)
                diff_percent = ((bigger_metric - smaller_metric) / smaller_metric) * 100
            difference[key] = round(diff_percent, 2)
            total_diff += diff_percent
        elif key == "energy":
            number1 = float(value)
            number2 = float(process2[key])
            percent_decrease = ((min(number1, number2) - max(number1, number2)) / min(number1, number2)) * 100
            difference[key] = round(percent_decrease, 2)
            total_diff += percent_decrease

    if total_diff > threshold:
        difference["better_process"] = "Process 1"
    elif total_diff < -threshold:
        difference["better_process"] = "Process 2"
    else:
        difference["better_process"] = "None"

    difference["total_diff"] = round(abs(total_diff), 2)
    print(process1)
    print(process2)
    print(difference)
    return difference



