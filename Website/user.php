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
                <h1 class="main_text">Naudotojas</h1>
        </div>
    </section>

    <section class="main_header">
        <div class="container">
            <h1 class="main_text" id="username"><?php echo ($_SESSION['USERINFO']->email) ?></h1>
            <br></br>
            <p>
                <b>PREMIUM</b> - Vieta susimokėti? Benefits?.
            </p>

        </div>
    </section>

</main>

<header class="page-header header container-fluid">
</header>

<script src="https://code.jquery.com/jquery-3.6.3.js" integrity="sha256-nQLuAZGRRcILA+6dMBOvcRh5Pe310sBpanc6+QBmyVM=" crossorigin="anonymous"></script>
<script src="js/bootstrap.min.js"></script>

</body>

</html>