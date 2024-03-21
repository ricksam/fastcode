<?php
$db_host = "localhost";
$db_user = "ricksam";
$db_password = "ricksam";
$db_name = "test";

    ini_set('display_errors', 1);
    error_reporting(E_ALL);

echo 'testedb';

$conn = mysqli_connect($db_host, $db_user, $db_password, $db_name);

if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}else{
    echo 'sucesso!';
}
?>
