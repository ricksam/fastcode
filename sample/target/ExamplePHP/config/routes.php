<?php
function route(string $path, callable $callback) {
  global $routes;
  $routes[$path] = $callback;
}

$routes = [];

route('/', function () {
  require './views/home.php';
});

route('/usuario', function () {
  require './views/usuario.php';
});

route('/api/usuario', function () {
    //echo 'api/usuario';
    if($_SERVER['REQUEST_METHOD']=='GET') {
        require './controllers/usuario/list.php';
    }
    if($_SERVER['REQUEST_METHOD']=='POST'){
        require './controllers/usuario/save.php';
    }
    if($_SERVER['REQUEST_METHOD']=='DELETE'){
        require './controllers/usuario/remove.php';
    }
});

route('/cadastro', function () {
  require './views/cadastro.php';
});

route('/login', function () {
  require './views/login.php';
});

route('/login/save', function () {
  require './views/login_post.php';
});

route('/about-us', function () {
  echo "About Us";
});

route('/404', function () {
  echo "Page not found";
});

function run() {
  global $routes;
  $uri = explode("?", $_SERVER['REQUEST_URI']) [0];
  //echo json_encode($_SERVER['QUERY_STRING']);
  $found = false;
  foreach ($routes as $path => $callback) {
    //echo $uri;
    if ($path !== $uri && $path.'/' !== $uri) {
        continue;
    }

    $found = true;
    $callback();
  }

  if (!$found) {
    $notFoundCallback = $routes['/404'];
    $notFoundCallback();
  }
}

run();
?>

