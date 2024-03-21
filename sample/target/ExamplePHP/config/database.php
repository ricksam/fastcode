<?php
// Configurações de conexão com o banco de dados (substitua pelos seus próprios dados)
$host = 'localhost';
$user = 'root';
$pwd = '';
$db = 'test';

// Função para estabelecer a conexão com o banco de dados
function db_connect()
{
    ini_set('display_errors', 1);
    error_reporting(E_ALL);
    global $host, $user, $pwd, $db;
    $conexao = mysqli_connect($host, $user, $pwd, $db);
    
    if (!$conexao) {
        die('Erro na conexão com o banco de dados: ' . mysqli_connect_error());
    }

    return $conexao;
}

function db_filter($sql)
{
    $conexao = db_connect();
    
    // Recuperar o usuário com o email fornecido
    $resultado = mysqli_query($conexao, $sql);

    $output=[];

    if ($resultado) {
        
        //$row = mysqli_fetch_assoc($resultado);
        while ($row = mysqli_fetch_assoc($resultado)) {
        // Faça algo com cada linha, por exemplo, exibir ou armazenar em um array
            //print_r($row); // Isso exibirá a linha como um array associativo
            array_push($output, $row);
        }
        
    }

    mysqli_close($conexao);
    //echo $output;
    return $output;
}

function db_exec($sql)
{
    $conexao = db_connect();

    if (mysqli_query($conexao, $sql)) {
        mysqli_close($conexao);
        return true;
    } else {
        mysqli_close($conexao);
        return false;
    }
}
?>

