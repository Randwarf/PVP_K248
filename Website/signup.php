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

$ERROR = "";

if (isset($_POST['register'])) {
    if (isset($_POST['email']) && isset($_POST['password']) && !empty($_POST['email']) && !empty($_POST['password'])){
        $data = array(
            "email" =>  $_POST['email'],
            "password" => $_POST['password'],
            "isPremium" => false
        );
        $data = json_encode($data);

        $curl = curl_init();
        curl_setopt_array($curl, array(
            CURLOPT_URL => "http://127.0.0.1:5000/create-user",
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
        
        if (isset($response->code)){
            $ERROR = $response->message;
        }
    } else {
        if (empty($_POST['email'])) {
            $ERROR = "Email cannot be empty";
        } else if (empty($_POST['password'])) {
            $ERROR = "Password cannot be empty";
        }
    }
}
?>
    
<main>
    <section class="main_info">
        <div class="main_header">
                <h1 class="main_text">Registracija</h1>
                <h4>Susikūrk savo paskyrą, kuri suteiks tau papildomo funkcionalumo!</h4>
        </div>
    </section>

    <section class="main_header">
        <div class="login-form">
            <form action="#" method="post">
                <?php echo $ERROR;?>
                <br>
                <div class="form-group">
                    <p class="loginsignup">E-paštas</p>
                    <input type="email" name="email" placeholder="E-paštas" va>
                </div>

                <div class="form-group">
                    <p class="loginsignup">Slaptažodis</p>
			        <input type="password" name="password" placeholder="Slaptažodis">
                </div>
                <br>
                <button class="btn btn-warning" name="register" type ="submit">Registruotis</button>
            </form>
        </div>
    </section>
</main>

<header class="page-header header container-fluid">
</header>

<script src="https://code.jquery.com/jquery-3.6.3.js" integrity="sha256-nQLuAZGRRcILA+6dMBOvcRh5Pe310sBpanc6+QBmyVM=" crossorigin="anonymous"></script>
<script src="js/bootstrap.min.js"></script>

</body>

</html>