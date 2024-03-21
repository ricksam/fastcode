
const table = 'User'

exports.seed = async function (knex) {
  return knex.transaction((transaction) => {
    knex(table).del().then(() => {
      transaction(table)
        .insert([
          { Name: 'User Test 1', Email: 'teste1@server.com.br', Password: 'password1' },
          { Name: 'User Test 2', Email: 'teste2@server.com.br', Password: 'password2' }
        ])
        .into(table)
        .then(transaction.commit)
        .catch(transaction.rollback)
    })
  })
}
