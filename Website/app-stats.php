

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
<!--            process name of the program goes here           -->
        </div>
    </section>
    <section class="main_header">
        <table id="stat-table" class="table">
            <tbody>
<!--            Stats of the app go here                 -->
            </tbody>
        </table>
        <br>
        <form action="popular.php">
            <button type="submit" class="btn btn-warning">Atgal</button>
        </form>
    </section>
    <script>
        const url_params = new URLSearchParams(window.location.search);
        const url_param_process = url_params.get('process');
        
        $(document).ready(function() {
            // Define the API endpoint URL and query parameters
            var endpointUrl = "http://127.0.0.1:5000/get-app";
            var queryParams = { process: url_param_process };

            var imgSrc = "assets/img/average.png"; // replace with the actual image source
            var img = $("<img width='25' height='25'>").attr("src", imgSrc);

            // Make the API request using jQuery's $.get() method
            $.get(endpointUrl, queryParams, function(response) {
            // If the request is successful, append the data to the table
                var process_div = $("#process");
                process_div.append($("<h1 class=\"main_text\">").text(response[0].process));
                process_div.append($("<h4>").text("programa"));

                var table = $("#stat-table");

                var tr = $("<tr>");
                tr.append($("<br>"))
                tr.append($("<h4 style='margin-bottom: 0px;'>").text(" Vidutinė ištestavimų informacija").prepend(img));
                tr.append($("<h4 style='font-weight: normal; font-size: 13px; margin-top: 0px'>").text("(iš viso atliktų testavimų skaičius šiai programai -  " + response[0].count + ")"));
                tr.append($("<br>"))
                table.append(tr);

                var tr2 = $("<tr class=\"stat-info\">");
                tr2.append($("<td>").text("Procesoriaus (CPU) užimtumas"));
                tr2.append($("<td>").text(response[0].cpu + "%"));
                table.append(tr2);

                var tr3 = $("<tr class=\"stat-info\">");
                tr3.append($("<td>").text("Atminties (RAM) užimtumas"));
                tr3.append($("<td>").text(response[0].ram + "%"));
                table.append(tr3);

                var tr4 = $("<tr class=\"stat-info\">");
                tr4.append($("<td>").text("Disko užimtumas"));
                tr4.append($("<td>").text(response[0].disk + "%"));
                table.append(tr4);

                var tr5 = $("<tr class=\"stat-info\">");
                tr5.append($("<td>").text("Energijos išnaudojimas"));
                tr5.append($("<td>").text(response[0].energy));
                table.append(tr5);
            }).fail(function() {
                // If the request fails, print an error message
                var process_div = $("#process");
                process_div.append($("<h1 class=\"main_text\">").text("Dar niekas tokios programos neištestavo"));
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