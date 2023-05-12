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

if (isset($_POST['email'])){

    $data = array(
        "email" =>  $_POST['email'],
        "password" => $_POST['password']
    );
    $data = json_encode($data);

    $curl = curl_init();
    curl_setopt_array($curl, array(
        CURLOPT_URL => "http://127.0.0.1:5000/login",
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
        $ERROR = "NETEISINGI DUOMENYS";
    }
    else{
        $_SESSION['token'] = $response;
        header("Location: /index.php");
    }

}

?>
    
<main>
    <section class="main_info">
        <div class="main_header">
                <h1 class="main_text">Prisijungimas</h1>
        </div>
    </section>

    <section class="main_header">
        <div class="login-form">
            <form action="#" method="post">
                <?php echo $ERROR;?>
                <p>E-paštas</p>
                <input type="text" name="email" placeholder="E-paštas">

                <p>Slaptažodis</p>
			    <input type="password" name="password" placeholder="Slaptažodis">
                <br>
                <button type ="submit">Prisijungti</button>
		    	<a class ="regis" href = "signup.php"> Registruotis</a>
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