<?php
$file = "./assets/app/Benchmarkerv01.exe";

if(!file_exists($file)) {header("Location: ./index.html");
die(); }

$type = filetype($file);
$today = date("F j, Y, g:i a");
$time = time();
// Send file headers
header("Content-type: $type");
header("Content-Disposition: attachment;filename=Benchmarkerv01.exe");
header("Content-Transfer-Encoding: binary");
header('Pragma: no-cache');
header('Expires: 0');
set_time_limit(0);
ob_clean();
flush();
readfile($file);
?>
