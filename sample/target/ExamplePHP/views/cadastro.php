<?php
output_header("Cadastro");
?>

<h1>Cadastro</h1>
<a href="/login">Login</a>
<form action="post.php" method="post">
    Nome: <input type="text" name="nome"><br>
    Email: <input type="email" name="email"><br>
    Senha: <input type="password" name="senha"><br>
    <input type="submit" value="Cadastrar">
</form>

<?php
output_footer();
?>

