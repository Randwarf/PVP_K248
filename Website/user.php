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

<script async src="assets/scripts/header.js"></script>  
<?php 
include("assets/include/header.php");
?>
    
<main>

    <section class="main_info">
        <div class="main_header">
                <h1 class="main_text">Vartotojas</h1>
        </div>
    </section>

    <section class="main_header">
        <div class="container">
            <h1 class="main_text" id="username"></h1>
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

</main>

<script>
    today = new Date();
    var expire = new Date();
    expire.setTime(today.getTime() + 1000*60*60*24*3); //3 dienas
    //user_id="MATVAI:)"
    document.cookie = "user_id=" + "MATVAI" + "; path=/" + "; expires="+expire.toUTCString();
    console.log("user_id=" + "MATVAI" + "; path=/" + "; expires="+expire.toUTCString());


    user_id="NULL"
    console.log(document.cookie);
    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].trim();
        console.log(cookie);
        if (cookie.startsWith("user_id=")) {
            user_id = cookie.substring("user_id=".length, cookie.length);
            break;
        }
}
    document.getElementById("username").innerHTML = user_id;

</script>

<header class="page-header header container-fluid">
</header>

<script src="https://code.jquery.com/jquery-3.6.3.js" integrity="sha256-nQLuAZGRRcILA+6dMBOvcRh5Pe310sBpanc6+QBmyVM=" crossorigin="anonymous"></script>
<script src="js/bootstrap.min.js"></script>

</body>

</html>