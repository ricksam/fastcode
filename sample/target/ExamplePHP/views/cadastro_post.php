<?php
require '../common/database.php';

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $nome = $_POST['nome'];
    $email = $_POST['email'];
    $senha = password_hash($_POST['senha'], PASSWORD_BCRYPT); // Salva a senha de forma segura

    echo 'gravando'.$nome.$email.$senha; 
    if (insereUsuario($nome, $email, $senha)) {
        echo "Cadastro realizado com sucesso!";
    } else {
        echo "Erro ao cadastrar";
    }
}
?>

