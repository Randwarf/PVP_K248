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
    <div class="main_header">
      <h1 class="main_text">Programos versijų istorija</h1>
      <h4>Čia galite rasti visas programos versijas ir jas atsisiųsti</h4>
    </div>
  </section>
  <section class="main_header">
    <table id="version-table" class="table">
      <tbody>
      <!--            App versions go here                 -->
      </tbody>
    </table>
    <br>
    <form action="index.php">
      <button type="submit" class="btn btn-warning">Atgal</button>
    </form>
  </section>
  <script>
    $(document).ready(function() {
      // Define the API endpoint URL and query parameters
      var endpointUrl = "http://127.0.0.1:5000/get-main-app-history";

      $.get(endpointUrl, function (response) {
        var table = $("#version-table");
        var tr1 = $("<tr>");
        tr1.append($("<td class='secondary_text'>").text("Versija"));
        tr1.append($("<td class='secondary_text'>").text("Išleidimo data"));
        table.append(tr1);
        // If the request is successful, append the data to the table
        response.forEach(function (row) {

          var tr2 = $("<tr class=\"stat-info\">");
          tr2.append($("<td>").text(row.version));
          var downloadBtn = $("<button class=\"btn btn-warning\">").text("Atsisiųsti");
          downloadBtn.attr("onclick", "location.href='" + row.dllink + "'");
          tr2.append($("<td>").text(row.date));
          tr2.append($("<td style='text-align: right'>").append(downloadBtn));
          table.append(tr2);
        });
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