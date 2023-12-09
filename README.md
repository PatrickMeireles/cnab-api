# Descrição

- Projeto desenvolvido baseado no desafio desse [repositório](https://github.com/Pagnet/desafio-back-end) escrito utilizando .net
- Projeto consiste em receber um arquivo CNAB pelo endpoint `[POST] api/v1/import-cnab` que vai receber o arquivo em um parâmetro chamado `file` via `form-data`.
- Após receber o arquivo, irá ser processada as linhas do arquivo e salvar no banco de dados.
- Haverá um Job que irá recuperar essas linhas de tempos em tempos e irá processar cada uma delas e efetuando as operações de transação de cada movimentação financeira.
- Possui 2 endpoints que permite ver as lojas e suas respectivas transações
    - `[GET] api/v1/stores`
    - `[POST] api/v1/stores/{id}/transactions`

# Instruções do desafio

Você recebeu um arquivo CNAB com os dados das movimentações finanaceira de várias lojas.
Precisamos criar uma maneira para que estes dados sejam importados para um banco de dados.

Sua tarefa é criar uma interface web que aceite upload do [arquivo CNAB](https://github.com/Pagnet/desafio-back-end/blob/master/CNAB.txt), normalize os dados e armazene-os em um banco de dados relacional e exiba essas informações em tela.

# Documentação do CNAB

| Descrição do campo  | Inicio | Fim | Tamanho | Comentário
| ------------- | ------------- | -----| ---- | ------
| Tipo  | 1  | 1 | 1 | Tipo da transação
| Data  | 2  | 9 | 8 | Data da ocorrência
| Valor | 10 | 19 | 10 | Valor da movimentação. *Obs.* O valor encontrado no arquivo precisa ser divido por cem(valor / 100.00) para normalizá-lo.
| CPF | 20 | 30 | 11 | CPF do beneficiário
| Cartão | 31 | 42 | 12 | Cartão utilizado na transação 
| Hora  | 43 | 48 | 6 | Hora da ocorrência atendendo ao fuso de UTC-3
| Dono da loja | 49 | 62 | 14 | Nome do representante da loja
| Nome loja | 63 | 81 | 19 | Nome da loja

# Documentação sobre os tipos das transações

| Tipo | Descrição | Natureza | Sinal |
| ---- | -------- | --------- | ----- |
| 1 | Débito | Entrada | + |
| 2 | Boleto | Saída | - |
| 3 | Financiamento | Saída | - |
| 4 | Crédito | Entrada | + |
| 5 | Recebimento Empréstimo | Entrada | + |
| 6 | Vendas | Entrada | + |
| 7 | Recebimento TED | Entrada | + |
| 8 | Recebimento DOC | Entrada | + |
| 9 | Aluguel | Saída | - |


# Tecnologias

- .Net 8
- Dapper
- Sqlite
- Quartz
- Ardalis