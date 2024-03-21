<?php
require './models/user.php';

echo '<br /> in login_post'.$_SERVER['REQUEST_METHOD'];

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    echo '<br /> in login post if POST';
    $email = $_POST['email'];
    $senha = $_POST['senha'];
    echo 'email:'.$email;
    echo 'senha:'.$senha;
    if (verificaLogin($email, $senha)) {
        echo "Login ok.";
        header('Location: /cadastro');
        die();
    }else{
        echo "Login falhou. Verifique suas credenciais.";
    }
}else{
    echo '<br /> in login post else GET';
}
?>

