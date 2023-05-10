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

$token = isset($_GET['token']) ? $_GET['token'] : '';
$data = json_encode(array('token' => $token));

$curl = curl_init();
curl_setopt_array($curl, array(
    CURLOPT_URL => "http://127.0.0.1:5000/create-user-confirmed",
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_FOLLOWLOCATION => true,
    CURLOPT_POST => true,
    CURLOPT_POSTFIELDS => $data,
    CURLOPT_HTTPHEADER => array(
        'Content-Type: application/json',
        "Content-Length ".strlen($data)
    )
));

$response = curl_exec($curl);
curl_close($curl);
$response = json_decode($response);
?>

<main>
    <section class="main_info">
        <div class="main_header">
            <?php if (isset($response->code) && $response->code == 200): ?>
            <h1 class="main_text">Registracijos patvirtinimas</h1>
            <h4>Jūsų paskyra sukūrta!</h4>
            <br>
            <form action="login.php">
                <button type="submit" class="btn btn-warning">Pereiti prie prisijungimo</button>
            </form>
            <?php elseif (isset($response->code) && $response->code == 402): ?>
                <h1 class="main_text">Registracija nepavyko</h1>
                <br>
                <h4>Pasibaigė galioti patvirtinimo nuorodos laikas. Registruokitės iš naujo.</h4>
                <form action="signup.php">
                    <button type="submit" class="btn btn-warning">Grįžti prie registracijos</button>
                </form>
            <?php else: ?>
                <h1 class="main_text">Registracija nepavyko</h1>
                <h4>Įvyko klaida.</h4>
                <br>
                <form action="signup.php">
                    <button type="submit" class="btn btn-warning">Grįžti prie registracijos</button>
                </form>
            <?php endif; ?>
        </div>
    </section>
</main>

<header class="page-header header container-fluid">
</header>

<script src="https://code.jquery.com/jquery-3.6.3.js" integrity="sha256-nQLuAZGRRcILA+6dMBOvcRh5Pe310sBpanc6+QBmyVM=" crossorigin="anonymous"></script>
<script src="js/bootstrap.min.js"></script>

</body>

</html>