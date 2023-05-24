<!DOCTYPE html>
<html lang="en">
<head>

    <title>eko-logika</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/style.css">

</head>
<body>

<?php 
include("assets/include/header.php");
?>
    <section class="main_header">
        <div class="container">
            <h1 class="main_text">Aplikacijos atsisiuntimo ir paleidimo instrukcija</h1>
            <br></br>
            <p>
               Pirmiausia atsisiunčiame nemokamą programą iš pradžios puslapio. 

            </p>
            <img src="assets/img/atsisiusti.png" alt="Atsiuntimo mygtukas" style="width:640px;height:320px;">
            <p>

                Atidarome atsisiųstą zip failą. 
            </p>
            <img src="assets/img/zipfailas.png" alt="Programos failas" style="width:740px;height:420px;">
            <br></br>
            <p>
                
                Pasirenkame aplanką Release, kuriame rasime vykdymo failą. 
            </p>
            <img src="assets/img/relaplankas.png" alt="Zip failo pagrindinis" style="width:640px;height:320px;">
            <br></br>
            <p>
                
                Atsidarę aplanką Release, pasirenkame programos failą Benchmarker ir paleidžiame programą. 
            </p>
            <img src="assets/img/programfailas.png" alt="Programos vykdymo failas" style="width:640px;height:320px;">
        </div>
    </section>
    <section class="main_header">
        <div class="container">
            <h1 class="main_text">Aplikacijos naudojimo instrukcija</h1>
            <br></br>
            <p>
                
                Atvėrę programą, pagrindiniame lange matome mygtuką "Start a new benchmark". Paspaudus, bus surasti visi veikiantys procesai.
            </p>
            <img src="assets/img/Main.png" alt="Pagrindinis langas" width=75%>
            <br></br>
            <p>
                
                Atsivėrusiame naujame lange išsirenkame norimą testuoti procesą.
            </p>
            <img src="assets/img/Process.png" alt="Procesų langas" width=75%>
            <br></br>
            <p>
                
                Šis procesas dabar yra testuojamas, o jo informacija saugoma. Proceso testavimas sustabdomas rankiniu būdu, tačiau verta pastebėti, kad jei testuosime trumpiau, nei 30 sekundžių, testavimas nebus užskaitytas.
            </p>
            <img src="assets/img/run.png" alt="testavimo langas" width=75%>
            <br></br>
            <p>
                
                Sustabdžius proceso testavimą, jo suvestinę galime matyti "History" lange.
            </p>
            <img src="assets/img/history.png" alt="history langas" width=75%>
        </section>
</main>

<header class="page-header header container-fluid">
</header>

<script src="https://code.jquery.com/jquery-3.6.3.js" integrity="sha256-nQLuAZGRRcILA+6dMBOvcRh5Pe310sBpanc6+QBmyVM=" crossorigin="anonymous"></script>
<script src="js/bootstrap.min.js"></script>

</body>

</html>