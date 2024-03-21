<?php
output_header("Home");
?>

<div id="app">
    <loading :active.sync="loading" loader="dots" color="#009688" :can-cancel="false"></loading>
    <h1 class="my-3" id="content">usuario</h1>
    <div class="row">
        <div class="my-1 col-md-4">
            <input class=" form-control form-control-sm" placeholder="buscar" v-model="search" />
        </div>
        <div class="my-1 col-md-8">
            <button class="float-end btn btn-sm btn-primary" @click="novo()">Adicionar</button>
        </div>
    </div>

    <div class="table-responsive">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>nome</th>
                    <th>email</th>
                    <th>senha</th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(item, index) in itemsFiltreds" :key="index">
                    <td>{{item.id}}</td>
                    <td>{{item.nome}}</td>
                    <td>{{item.email}}</td>
                    <td>{{item.senha}}</td>
                    <td>
                        <button class="btn btn-sm btn-warning" @click="editar(index)">Editar</button>
                    </td>
                    <td>
                        <button class="btn btn-sm btn-danger" @click="desejaRemover(index)">Remover</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="editModal" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <!--div class="modal-header">
                    <h5 class="modal-title" id="editModalLabel">usuario</h5>
                </div-->
                <div class="modal-body row">
                    <div class="col-sm-6 my-2">
                        <label>nome</label>
                        <input type="text"  class="form-control  form-control-sm" v-model="form.nome" placeholder="nome" />
                    </div>
                    <div class="col-sm-6 my-2">
                        <label>email</label>
                        <input type="text"  class="form-control  form-control-sm" v-model="form.email" placeholder="email" />
                    </div>
                    <div class="col-sm-6 my-2">
                        <label>senha</label>
                        <input type="text"  class="form-control  form-control-sm" v-model="form.senha" placeholder="senha" />
                    </div>
                    <div v-if="error" class="alert alert-danger">
                        {{error}}
                        <button type="button" class="float-end btn-close" data-dismiss="alert" aria-label="Close"></button>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-sm btn-secondary" @click="closeModal('editModal')">Fechar</button>
                    <button type="button" class="btn btn-sm btn-primary" @click="salvar()">Salvar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="removeModal" tabindex="-1" role="dialog" aria-labelledby="removeModalLabel" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    Tem certeza que deseja remover o registro:
                    <input class="my-2 form-control  form-control-sm" v-model="form.nome" disabled="disabled" />
                    <div v-if="error" class="alert alert-danger">
                        {{error}}
                        <button type="button" class="float-end btn-close" data-dismiss="alert" aria-label="Close"></button>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-sm btn-secondary" @click="closeModal('removeModal')">Fechar</button>
                    <button type="button" class="btn btn-sm btn-danger" @click="remover()">Remover</button>
                </div>
            </div>
        </div>
    </div>
</div>




 <script>
        const api = axios.create({ baseURL: 'api/', headers: { 'token': localStorage.getItem('token') }, timeout: 0 });
        const app = new Vue({
            el: "#app",
            data: {
                loading: false,
                items: [],
                form: {},
                search:'',
                error:''
            },
            computed: {
                itemsFiltreds() {
                    if (this.search) {
                        return this.items.filter(q => q.nome.toLowerCase().includes(this.search.toLowerCase()));
                    } else {
                        return this.items;
                    }
                }
            },
            mounted() {
                this.load();
            },
            methods: {
                load() {
                    this.listar();
                },
                formatDate(strDate) {
                    return moment(strDate).format("DD/MM/YYYY HH:mm");
                },
                listar() {
                    this.loading=true;
                    api.get("usuario").then(res => {
                        console.log(res.data);
                        this.loading=false;
                        this.items = res.data;
                        console.log("items:", this.items);
                    })
                },
                novo() {
                    this.error = '';
                    this.form = { ativo: true }
                    $("#editModal").modal("show");
                },
                editar(index) {
                    this.error = '';
                    this.form = JSON.parse(JSON.stringify(this.items[index]));
                    $("#editModal").modal("show");
                },
                desejaRemover(index) {
                    this.error = '';
                    this.form = JSON.parse(JSON.stringify(this.items[index]));
                    $("#removeModal").modal("show");
                },
                remover() {
                    api.delete("usuario?id=" + this.form.id).then(res => {
                        if (res.data.success) {
                            this.listar();
                            $("#removeModal").modal("hide");
                        } else {
                            this.error = res.data.message;
                        }
                    })
                },
                salvar() {
                    console.log('salvar');
                    console.log("this.form:", JSON.stringify(this.form));
                    api.post("usuario", this.form).then(res => {
                        console.log(res.data);
                        if (res.data.success) {
                            $("#editModal").modal("hide");
                            this.listar();
                        } else {
                            this.error = res.data.message;
                        }
                    })
                },
                closeModal(id){
                    $("#"+id).modal('hide');
                }
            }
        });
    </script>

<?php
output_footer();
?>

