<?php
output_header("Login");
?>

<h1>Login</h1>
<form action="login/save" method="post">
    Email: <input type="email" name="email"><br>
    Senha: <input type="password" name="senha"><br>
    <input type="submit" value="Entrar">
</form>

<?php
output_footer();
?>

