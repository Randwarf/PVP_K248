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
                <h1 class="main_text">Programų ištestavimas</h1>
                <a id="download-latest-version" class="btn btn-warning" role="button" download>Atsisiųsti nemokamą programą</a>
                <a id="version-history" class="btn btn-warning" onclick="location.href='version-history.html'" role="button" download>Versijų istorija</a>
        </div>
    </section>

    <section class="info_cards">
        <div class="container">
            <div class="row">
                <div class="col-lg-4 mb-4">
                    <div class="card" style="width: 14rem;">
                        <div class="card-body">
                            <h5 id="tested-apps-count" class="card-title"></h5>
                            <p class="card-text">Ištestuotų programų</p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 mb-4">
                    <div class="card" style="width: 14rem;">
                        <div class="card-body">
                            <h5 id="tests-count" class="card-title"></h5>
                            <p class="card-text">Atlikti testavimai</p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 mb-4">
                    <div class="card" style="width: 14rem;">
                        <div class="card-body">
                            <h5 id="unique-users-count" class="card-title"></h5>
                            <p class="card-text">Unikalūs naudotojai</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="main_header">
        <div class="container">
            <h1 class="main_text">Žaliasis programavimas</h1>
            <br></br>
            <p>
                <b>Žaliasis programavimas</b> - tai programavimo metodas, kurio tikslas yra sukurti ne tik efektyvias programas, atliekančias savo funkciją, bet ir mažinti savo, kompiuterių ir kitų technologijų poveikį aplinkai.
            </p>
            <p>
                <b>Šis programavimo metodas tampa vis svarbesnis dėl kelių priežasčių.</b>
                Pirmiausia, klimato kaita yra didelis ir visuotinis iššūkis visuomenei.
                Kompiuteriai ir technologijos, kuriuos naudojame kasdien, sudaro didelę dalį mūsų ekologinio pėdsako, todėl mums reikia susitelkti į jų poveikio sumažinimą.
                Be to, žaliasis programavimas taip pat gali padėti mažinti energijos sąnaudas ir sumažinti išlaidas, taigi tai yra pranašumas tiek ekologiniu, tiek ekonominiu požiūriu.
            </p>
        </div>
    </section>
    <script>
        $(document).ready(function() {
            var appCountCard = $("#tested-apps-count");
            var testCountCard = $("#tests-count");
            var usersCard = $("#unique-users-count");

            var endpointUrl = "http://127.0.0.1:5000/get-overall-stats"
            $.get(endpointUrl, function (response) {
                response = response[0];
                appCountCard.append(response.apps_count);
                testCountCard.append(response.tests_count);
                usersCard.append(response.users_count)
            }).fail(function () {
                appCountCard.append("0");
                testCountCard.append("0");
                usersCard.append("0")
            });

            var endpointUrl = "http://127.0.0.1:5000/get-main-app-history"
            var queryParams = {latest: ""};
            var downloadButton = $("#download-latest-version");
            $.get(endpointUrl, queryParams, function (response) {
                response = response[0];
                downloadButton.attr("onclick", "location.href='" + response.dllink + "'");
            })
        });
    </script>
</main>

<header class="page-header header container-fluid">
</header>

<script src="https://code.jquery.com/jquery-3.6.3.js" integrity="sha256-nQLuAZGRRcILA+6dMBOvcRh5Pe310sBpanc6+QBmyVM=" crossorigin="anonymous"></script>
<script src="js/bootstrap.min.js"></script>

</body>

</html>