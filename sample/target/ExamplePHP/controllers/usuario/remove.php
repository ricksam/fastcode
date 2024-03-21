<?php
    require './models/user.php';
    //echo $_GET['id'];
    //$post = file_get_contents('php://input');
    //echo $post;
    $res["success"] = apagarUsuario($_GET['id']);
    header('Content-Type: application/json');
    echo json_encode($res);
?>
