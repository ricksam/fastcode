<?php
class MinhaClasse {
    public $campo1;
    public $campo2;
    private $campo3;
}

$objeto = new MinhaClasse();

$campos = get_object_vars($objeto);
$nomes_dos_campos = array_keys($campos);

print_r(json_encode($nomes_dos_campos) );
