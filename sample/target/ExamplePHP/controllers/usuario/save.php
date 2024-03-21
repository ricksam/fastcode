<?php
    require './models/user.php';
    $post = file_get_contents('php://input');
    //echo json_encode(get_object_vars($post)) ;
    $res["success"] = salvarUsuario($post);
    header('Content-Type: application/json');
    echo json_encode($res);
?>
