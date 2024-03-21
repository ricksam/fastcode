<?php
function listarUsuarios()
{
    return db_filter("SELECT * FROM usuarios");
}

function salvarUsuario($post){
    $entity = json_decode($post);
    if(isset($entity->id) && $entity->id != 0){
        atualizaUsuario($entity->id, $entity->nome, $entity->email);
    }
    else{
        $nova_senha = password_hash($entity->senha, PASSWORD_BCRYPT);
        insereUsuario($entity->nome, $entity->email, $nova_senha);
    }
    return true;
}

// Função para inserir um novo usuário no banco de dados
function insereUsuario($nome, $email, $senha)
{
    $conexao = db_connect();
    // Lembre-se de escapar os valores para evitar injeção de SQL
    $nome = mysqli_real_escape_string($conexao, $nome);
    $email = mysqli_real_escape_string($conexao, $email);

    // Inserir o usuário
    $sql = "INSERT INTO usuarios (nome, email, senha) VALUES ('$nome', '$email', '$senha')";

    return db_exec($sql);
}

function atualizaUsuario($id, $nome, $email)
{
    $conexao = db_connect();
    // Lembre-se de escapar os valores para evitar injeção de SQL
    $nome = mysqli_real_escape_string($conexao, $nome);
    $email = mysqli_real_escape_string($conexao, $email);

    // Inserir o usuário
    $sql = "UPDATE usuarios SET nome='$nome', email='$email' WHERE id='$id' ";

    return db_exec($sql);
}

function apagarUsuario($id)
{
    $sql = "DELETE FROM usuarios WHERE id='$id' ";
    return db_exec($sql);
}

// Função para verificar o login do usuário
function verificaLogin($email, $senha)
{
    $conexao = conectarBanco();
    
    // Lembre-se de escapar o email para evitar injeção de SQL
    $email = mysqli_real_escape_string($conexao, $email);

    // Recuperar o usuário com o email fornecido
    $sql = "SELECT senha FROM usuarios WHERE email = '$email'";
    $resultado = mysqli_query($conexao, $sql);

    if ($resultado) {
        $row = mysqli_fetch_assoc($resultado);

        if ($row && password_verify($senha, $row['senha'])) {
            mysqli_close($conexao);
            return true;
        }
    }

    mysqli_close($conexao);
    return false;
}
?>
