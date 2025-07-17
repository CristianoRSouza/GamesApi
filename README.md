# Quake Log Parser API

## Descrição

Este projeto implementa um parser para o arquivo de log do servidor Quake 3 Arena (`games.log`). O parser agrupa os dados de cada jogo, coletando informações detalhadas sobre as mortes (kills), jogadores participantes e estatísticas relevantes para cada partida.

A API RESTful expõe endpoints para consulta dos jogos processados, retornando um relatório estruturado com o total de kills, lista de jogadores e kills por jogador para cada jogo.

## Funcionalidades

- Leitura e processamento do arquivo de log do Quake 3.
- Identificação e separação de múltiplos jogos dentro do mesmo arquivo.
- Contabilização de kills totais, kills por jogador e registro de jogadores.
- Tratamento especial para kills causadas pelo `<world>`.
- Exposição via API REST com os endpoints:
  - `GET /api/games` — retorna a lista de todos os jogos.
  - `GET /api/games/{id}` — retorna detalhes de um jogo específico.
- Implementação de tratamento de erros na leitura do arquivo (tentativas de retry).
- Testes unitários cobrindo controllers e serviços.

## Tecnologias e Ferramentas

- C# .NET 6/7 (ASP.NET Core Web API)
- xUnit + Moq para testes unitários
- FluentAssertions para asserts mais expressivos
- Injeção de dependência para serviços
- Padrões RESTful para endpoints

## Testes Unitários

- Os testes estão localizados nas pastas `GamesTests.TestsControllers` e `GamesTests.TestsServices`.
- Utilizam mocks para simular a leitura de arquivo e parsing.
- Cobrem os principais cenários, incluindo:
  - Retorno de jogos válidos.
  - Retorno de jogo por ID existente e não existente.
  - Tratamento de exceções na leitura de arquivo.
  - Parsing correto de múltiplos jogos.
 
  - ## Melhorias Futuras

- Implementar **Fluent Validation** para garantir validações consistentes, reutilizáveis e fáceis de manter, aumentando a robustez da aplicação.
- Utilizar **DTOs (Data Transfer Objects)** para encapsular os dados trafegados pela API, evitando exposição desnecessária de informações internas e melhorando a segurança e clareza dos contratos.
- Integrar um **ORM (Object-Relational Mapper)**, como o **Entity Framework**, para facilitar a persistência dos dados, garantir integridade e simplificar operações com o banco, além de permitir futuras evoluções no modelo de dados com mais agilidade.


