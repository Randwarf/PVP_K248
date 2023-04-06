

<!DOCTYPE html>
<html lang="en">
<head>

    <title>eko-logika</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>


</head>
<body>

<?php 
include("assets/include/header.php");
?>

<main>
    <section class="main_info">
        <div id="process" class="main_header">
        </div>
    </section>
    <br>
    <section>
        <div class="main_header">
            <table id="stat-table1" class="table">
                <tbody>
                <!-- Stats of the app go here -->
                </tbody>
            </table>
            <br>
            <form action="compare.php">
                <button type="submit" class="btn btn-warning">Atgal</button>
            </form>
        </div>
    </section>
    <script>
        const url_params = new URLSearchParams(window.location.search);
        const url_param_process = url_params.get('process1');
        const url_param_process2 = url_params.get('process2');
        
        $(document).ready(function() {
            // Define the API endpoint URL and query parameters
            var endpointUrl = "http://127.0.0.1:5000/get-app";
            var endpointUrl2 = "http://127.0.0.1:5000/calc-diff";
            var queryParams = { process: url_param_process };
            var queryParams2 = { process: url_param_process2 };

            const request1 = $.get(endpointUrl, queryParams);
            const request2 = $.get(endpointUrl, queryParams2);
            $.when(request1, request2).done(function(response1, response2){
                var queryParams3 = { process1: response1[0][0], process2: response2[0][0]};
                const request3 = $.get(endpointUrl2, queryParams3).done(function(response3){
                    // Response data for the first process
                    const data1 = response1[0];

                    // Response data for the second process
                    const data2 = response2[0];

                    // Response data for difference calculations
                    const data3 = response3;

                    // If the request is successful, append the data to the table
                    var process1_div = $("#process");
                    process1_div.append($("<h1 class=\"main_text\">").text("Programų palyginimo ataskaita"));
                    process1_div.append($("<h4>").text("Surask žalesnę ir tvaresnę programą!"));

                    // If the request is successful, append the data to the table
                    // var process1_div = $("#process");
                    // process1_div.append($("<h1 class=\"main_text\">").text(data1[0].process));
                    // process1_div.append($("<h4>").text("programa"));
                    //
                    // var process2_div = $("#process2");
                    // process2_div.append($("<h1 class=\"main_text\">").text(data2[0].process));
                    // process2_div.append($("<h4>").text("programa"));


                    var table1 = $("#stat-table1");

                    var tr0 = $("<tr>");
                    tr0.append($("<br>"));
                    tr0.append($("<td class=\"compare-process-names\">").text(data1[0].process));
                    tr0.append($("<td class=\"compare-process-names\">").text(data2[0].process));
                    table1.append(tr0);

                    var tr1 = $("<tr>");
                    tr1.append($("<br>"));
                    tr1.append($("<td class='stat-info-count'>").text("(atliktų testavimų skaičius -  " + data1[0].count + ")"));
                    tr1.append($("<td class='stat-info-count'>").text("(atliktų testavimų skaičius -  " + data2[0].count + ")"));
                    table1.append(tr1);

                    var tr1 = $("<tr class=\"stat-info-average\">");
                    tr1.append($("<td>").text("Vidutiniškas įvertis"));
                    if (data3['better_process'] == "Process 1") {
                        tr1.append($("<td>").html("<img src='assets/img/up_arrow.png' width='12' height='12' />" + "<span class='calc-diff-percentage'>" + Math.abs(data3['avg_diff']) + "%" + "</span>"));
                        tr1.append($("<td>"));
                    } else {
                        tr1.append($("<td>"));
                        tr1.append($("<td>").html("<img src='assets/img/up_arrow.png' width='12' height='12' />" + "<span class='calc-diff-percentage'>" + Math.abs(data3['avg_diff']) + "%" + "</span>"));
                    }
                    table1.append(tr1);

                    var tr2 = $("<tr class=\"stat-info\">");
                    tr2.append($("<td>").text("Procesoriaus (CPU) užimtumas"));
                    if (data1[0].cpu > data2[0].cpu) {
                        tr2.append($("<td>").text(data1[0].cpu + "%"));
                        tr2.append($("<td>").html(data2[0].cpu + "%" + " " + "<span class='calc-diff-percentage'>" + "<img src='assets/img/up_arrow.png' width='12' height='12' />" + Math.abs(data3['cpu']) + "%" + "</span>"));
                    }
                    else if (data1[0].cpu < data2[0].cpu) {
                        tr2.append($("<td>").html(data1[0].cpu + "%" + " " + "<span class='calc-diff-percentage'>" + "<img src='assets/img/up_arrow.png' width='12' height='12' />" + Math.abs(data3['cpu']) + "%" + "</span>"));
                        tr2.append($("<td>").text(data2[0].cpu + "%"));
                    } else {
                        tr2.append($("<td>").text(data1[0].cpu + "%"));
                        tr2.append($("<td>").text(data2[0].cpu + "%"));
                    }
                    table1.append(tr2);

                    var tr3 = $("<tr class=\"stat-info\">");
                    tr3.append($("<td>").text("Atminties (RAM) užimtumas"));
                    if (data1[0].ram > data2[0].ram) {
                        tr3.append($("<td>").text(data1[0].ram + "%"));
                        tr3.append($("<td>").html(data2[0].ram + "%" + " " + "<img src='assets/img/up_arrow.png' width='12' height='12' />" + "<span class='calc-diff-percentage'>" + Math.abs(data3['ram']) + "%" + "</span>"));
                    }
                    else if (data1[0].ram < data2[0].ram) {
                        tr3.append($("<td>").html(data1[0].ram + "%" + " " + "<img src='assets/img/up_arrow.png' width='12' height='12' />" + "<span class='calc-diff-percentage'>" + Math.abs(data3['ram']) + "%" + "</span>"));
                        tr3.append($("<td>").text(data2[0].ram + "%"));
                    } else {
                        tr3.append($("<td>").text(data1[0].ram + "%"));
                        tr3.append($("<td>").text(data2[0].ram + "%"));
                    }
                    table1.append(tr3);

                    var tr4 = $("<tr class=\"stat-info\">");
                    tr4.append($("<td>").text("Disko užimtumas"));
                    if (data1[0].disk > data2[0].disk) {
                        tr4.append($("<td>").text(data1[0].disk + "%"));
                        tr4.append($("<td>").html(data2[0].disk + "%" + " " + "<img src='assets/img/up_arrow.png' width='12' height='12' />" + "<span class='calc-diff-percentage'>" + Math.abs(data3['disk']) + "%" + "</span>"));
                    }
                    else if (data1[0].disk < data2[0].disk) {
                        tr4.append($("<td>").html(data1[0].disk + "%" + " " + "<img src='assets/img/up_arrow.png' width='12' height='12' />" + "<span class='calc-diff-percentage'>" + Math.abs(data3['disk']) + "%" + "</span>"));
                        tr4.append($("<td>").text(data2[0].disk + "%"));
                    } else {
                        tr4.append($("<td>").text(data1[0].disk + "%"));
                        tr4.append($("<td>").text(data2[0].disk + "%"));
                    }
                    table1.append(tr4);

                    var tr4 = $("<tr class=\"stat-info\">");
                    tr4.append($("<td>").text("Energijos išnaudojimas"));
                    if (data1[0].energy > data2[0].energy) {
                        tr4.append($("<td>").text(data1[0].energy));
                        tr4.append($("<td>").html(data2[0].energy + " " + "<img src='assets/img/up_arrow.png' width='12' height='12' />" + "<span class='calc-diff-percentage'>" + Math.abs(data3['energy']) + "%" + "</span>"));
                    }
                    else if (data1[0].energy < data2[0].energy) {
                        tr4.append($("<td>").html(data1[0].energy + " " + "<img src='assets/img/up_arrow.png' width='12' height='12' />" + "<span class='calc-diff-percentage'>" + Math.abs(data3['energy']) + "%" + "</span>"));
                        tr4.append($("<td>").text(data2[0].energy));
                    } else {
                        tr4.append($("<td>").text(data1[0].energy));
                        tr4.append($("<td>").text(data2[0].energy));
                    }
                    table1.append(tr4);
                });
            }).fail(function() {
                // If the request fails, print an error message
                var process_div = $("#process");
                process_div.append($("<h1 class=\"main_text\">").text("Bent viena programa dar nebuvo ištestuota"));
                process_div.append($("<h2>").text("Ištestuok ją pats ir būk pirmas!"));
            });
        });
    </script>
</main>
<header class="page-header header container-fluid">
</header>

<script src="https://code.jquery.com/jquery-3.6.3.js" integrity="sha256-nQLuAZGRRcILA+6dMBOvcRh5Pe310sBpanc6+QBmyVM=" crossorigin="anonymous"></script>
<script src="js/bootstrap.min.js"></script>

</body>

</html>