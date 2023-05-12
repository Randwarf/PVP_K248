<?php

session_start();
session_destroy();

$data = array(
    "token" =>  $_SESSION['token']
);
$data = json_encode($data);

$curl = curl_init();
curl_setopt_array($curl, array(
    CURLOPT_URL => "http://127.0.0.1:5000/logout",
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

header("Location: /index.php");

?>