def calc_diff_percentages(process1, process2):
    difference = {}
    for key, value in process1.items():
        if (key == "cpu" or key == "ram" or key == "disk"):
            percentage1 = float(value)
            percentage2 = float(process2[key])
            percentage_diff = float(percentage2) - float(percentage1)
            difference[key] = round(percentage_diff, 2)
        if (key == "energy"):
            number1 = float(value)
            number2 = float(process2[key])
            percent_decrease = ((min(number1, number2) - max(number1, number2)) / min(number1, number2)) * 100
            difference[key] = round(percent_decrease, 2)
    return difference