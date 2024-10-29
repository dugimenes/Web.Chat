# Feedback do Instrutor

#### 28/10/24 - Revisão Inicial - Eduardo Pires

## Pontos Positivos:

- Boa separação de responsabilidades.
- Arquitetura enxuta de acordo com a complexidade do projeto
- Demonstrou conhecimento em Identity e JWT
- Aplicou corretamente a associação do User com o Autor
- Usou bem o conceito de serviços para ambas aplicações
- Mostrou entendimento do ecossistema de desenvolvimento em .NET

## Pontos Negativos:

- Não vi a validação da role Admin para edição de post
- Aparentemente qualquer um pode deletar comentários
- Ao editar um post o novo usuário passa a ser o dono do post (via API qualquer um pode editar sem validações)
- Sanitizar o uso de trycatch espalhado em controllers.
- Outras responsabilidades poderiam também estar em serviços ao invés de estar direto na controller (ex CRUD Posts)

## Sugestões:

- Evoluir o projeto para as necessidades solicitadas no escopo.

## Problemas:

- Não consegui executar a aplicação de imediato na máquina. É necessário que o Seed esteja configurado corretamente, com uma connection string apontando para o SQLite.

  **P.S.** As migrations precisam ser geradas com uma conexão apontando para o SQLite; caso contrário, a aplicação não roda.
