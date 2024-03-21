<?php
function scripts_vue(){
    echo "﻿<!-- https://vuejs.org/v2/guide/deployment.html -->
        <script src='//cdn.jsdelivr.net/npm/vue/dist/vue.js'></script>
        <script src='//unpkg.com/axios/dist/axios.min.js'></script>
        <script src='//cdn.jsdelivr.net/npm/vue-loading-overlay@3'></script>
        <script src='//cdn.jsdelivr.net/npm/v-mask/dist/v-mask.min.js'></script>
        <script src='//cdn.jsdelivr.net/npm/v-money/dist/v-money.min.js'></script>
        <!--Moment.js-->
        <!--Moment.js-->
        <script src='https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.22.2/moment.min.js'></script>
        <script src='https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.22.2/moment.js'></script>
        <script src='https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.22.2/moment-with-locales.js'></script>
        <script src='https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.22.2/moment-with-locales.min.js'></script>
        <!-- SCRIPT PRIVALLT -->
        <script type='text/javascript' id='PrivallyApp' src='https://app.privally.global/app.js' pid='77e29b-59dcea' async></script>
        <!-- SCRIPT PRIVALLT -->
        <!-- or point to a specific vue-select release -->
        <script src='https://unpkg.com/vue-select@3.0.0'></script>
        <link rel='stylesheet' href='https://unpkg.com/vue-select@3.0.0/dist/vue-select.css'>
        <script>
            Vue.component('loading', VueLoading);

            Vue.use(VueMask.VueMaskPlugin);
            Vue.directive('mask', VueMask.VueMaskDirective);
            Vue.filter('VMask', VueMask.VueMaskFilter)

            Vue.use(VMoney.VMoney, {precision: 2})

            //V-Select
            Vue.component('v-select', VueSelect.VueSelect)
        </script>
        <link rel='stylesheet' href='//cdn.jsdelivr.net/npm/vue-loading-overlay@3/dist/vue-loading.css'>";
}

function output_header($title, $styles="")
{
    $vue=scripts_vue();
    echo "﻿<!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='utf-8' />
            <meta name='viewport' content='width=device-width, initial-scale=1.0' />
            <title>$title - WebApplication1</title>
            <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet' integrity='sha384-GLhlTQ8iRABdZLl6O3oVMWSktQOp6b7In1Zl3/Jr59b6EGGoI1aFkw7cmDA6j6gD' crossorigin='anonymous'>
            <link rel='stylesheet' href='publics/css/site.css' />
            $styles
            $vue
        </head>
        <body>
            <header>
                <nav class='navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3'>
                    <div class='container'>
                        <a class='navbar-brand' asp-area='' asp-controller='Home' asp-action='Index'>WebApplication1</a>
                        <button class='navbar-toggler' type='button' data-toggle='collapse' data-target='.navbar-collapse' aria-controls='navbarSupportedContent'
                                aria-expanded='false' aria-label='Toggle navigation'>
                            <span class='navbar-toggler-icon'></span>
                        </button>
                        <div class='navbar-collapse collapse d-sm-inline-flex justify-content-between'>
                            <ul class='navbar-nav flex-grow-1'>
                                <li class='nav-item'>
                                    <a class='nav-link text-dark' asp-area='' asp-controller='Home' asp-action='Index'>Home</a>
                                </li>
                                <li class='nav-item dropdown'>
                                  <button class='btn dropdown-toggle' data-bs-toggle='dropdown' aria-expanded='false'>
                                    Cruds
                                  </button>
                                  <ul class='dropdown-menu'>
                                    <li><a class='dropdown-item' href='usuario'>Usuários</a></li>
                                  </ul>
                                </li>
                                <li class='nav-item'>
                                    <a class='nav-link text-dark' asp-area='' asp-controller='Home' asp-action='Privacy'>Privacy</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>
            <div class='container'>
                <main role='main' class='pb-3'>";
}

function output_footer($scripts="")
{
    echo "</main>
        </div>

        <footer class='border-top footer text-muted'>
            <div class='container'>
                &copy; 2022 - WebApplication1 - <a asp-area='' asp-controller='Home' asp-action='Privacy'>Privacy</a>
            </div>
        </footer>
        <script src='publics/lib/jquery/dist/jquery.min.js'></script>
        <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js' integrity='sha384-w76AqPfDkMBDXo30jS1Sgez6pr3x5MlQ1ZAGC+nuZB+EYdgRZgiwxhTBTkF7CXvN' crossorigin='anonymous'></script>
        <script src='publics/js/site.js' asp-append-version='true'></script>
        $scripts
    </body>
    </html>";
}
?>
