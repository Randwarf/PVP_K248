def calc_diff_percentages(process1, process2):
    process1_perc = []
    process2_perc = []
    difference = {}
    total_diff = 0

    for key, value in process1.items():
        if key == "cpu" or key == "disk" or key == "energy" or key == "ram":
            percentage1 = float(value)
            percentage2 = float(process2[key])
            if percentage1 == percentage2:
                diff_percent = 0
            else:
                bigger_metric = max(percentage1, percentage2)
                smaller_metric = min(percentage1, percentage2)
                if key != "energy":
                    if smaller_metric != 0:
                        diff_percent = ((abs(bigger_metric - smaller_metric)) / smaller_metric) * 100
                    else:
                        diff_percent = (abs(bigger_metric - smaller_metric)) * 100
                else:
                    if smaller_metric != 0:
                        diff_percent = (abs(bigger_metric - smaller_metric))
                    else:
                        diff_percent = (abs(bigger_metric - smaller_metric))

            difference[key] = round(abs(diff_percent))
            total_diff += abs(diff_percent)

            if percentage1 > percentage2:
                process2_perc.append(round(abs(diff_percent)))
            elif percentage2 > percentage1:
                process1_perc.append(round(abs(diff_percent)))

    avg_diff = total_diff / (len(process1.items())-2)
    avg_diff_proc1 = sum(process1_perc) / (len(process1.items())-2)
    avg_diff_proc2 = sum(process2_perc) / (len(process1.items())-2)

    if avg_diff_proc1 > avg_diff_proc2:
        difference["better_process"] = "Process 1"
    elif avg_diff_proc1 < avg_diff_proc2:
        difference["better_process"] = "Process 2"
    else:
        difference["better_process"] = "Equal"
    difference["avg_diff"] = round(avg_diff)
    return difference