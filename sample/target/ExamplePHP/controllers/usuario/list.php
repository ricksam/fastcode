<?php
    require './models/user.php';
    $users = listarUsuarios();
    header('Content-Type: application/json');
    echo json_encode($users);
?>
