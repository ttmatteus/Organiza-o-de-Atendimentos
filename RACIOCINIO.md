# Raciocínio da Solução — Organização de Atendimentos

## Parte 1: Modelagem do Problema

### 1. Classificação do Problema

**Classificação:**
Problema de **Bin Packing com múltiplas restrições** (*Multi-Constrained Bin Packing*) junto de características de **Task Scheduling**.

### Justificativa

O problema basicamente consiste em distribuir atendimentos dentro de sessões de consultórios respeitando limites de tempo.

Cada atendimento possui uma duração fixa em minutos, funcionando como o “peso” do item que precisa ser encaixado em um espaço disponível.

Já os consultórios possuem capacidade limitada:

* Sessão da manhã → 210 minutos
* Sessão da tarde → 210 minutos

O objetivo é:

* organizar os atendimentos;
* respeitar os limites de horário;
* e utilizar o menor número possível de consultórios.

Além disso:

* os atendimentos não podem ser divididos;
* cada sessão possui sequência temporal;
* e existe separação estrutural entre manhã e tarde.

Por isso o problema não é um bin packing simples, porque cada consultório possui **dois compartimentos independentes** (manhã e tarde).

---

### 2. Analogia com Problemas Clássicos

### Bin Packing

A analogia mais próxima é imaginar:

* os atendimentos como objetos;
* e os consultórios como caixas com capacidade limitada.

Cada atendimento ocupa uma quantidade de tempo, e precisamos encaixar tudo utilizando o menor número de consultórios possível.

A diferença é que:

* cada “caixa” possui dois compartimentos;
* manhã e tarde.

---

### Activity Selection / Scheduling

Também existe semelhança com problemas clássicos de escalonamento de tarefas.

Nesse cenário:

* os atendimentos são as atividades;
* as sessões são os intervalos disponíveis;
* e o sistema precisa encaixar tudo sem ultrapassar os limites.

---

### 3. Estruturas de Dados Escolhidas

```csharp
public class Appointment
{
    public string Name { get; }
    public int Duration { get; }
    public bool IsExpressed { get; }
}

public class Session
{
    public SessionType Type { get; }
    public int MaxDuration { get; }
    public List<(Appointment, string)> Appointments { get; }
    public int UsedTime { get; }
}

public class Office
{
    public int Id { get; }
    public Session MorningSession { get; }
    public Session AfternoonSession { get; }
}

public class Clinic
{
    public List<Office> Offices { get; }
}
```

### Por que usar essas estruturas?

### Appointment

Representa um atendimento individual.

Ela encapsula:

* nome;
* duração;
* e se é expresso ou não.

Além disso, como as propriedades são somente leitura (`get`), os dados ficam imutáveis após criação do objeto.

Isso evita alteração acidental.

---

### Session

Responsável por controlar:

* tempo usado;
* validação de espaço disponível;
* sequência de horários.

Ela mantém regras importantes do sistema:

* impedir exceder o limite;
* controlar início e fim dos horários;
* gerar agenda formatada.

Sem essa classe, boa parte da lógica ficaria espalhada pelo código.

---

### Office

Funciona como agregador das sessões.

Ela decide:

* em qual sessão um atendimento será colocado;
* e se ainda existe espaço disponível.

---

### Clinic

Responsável pela organização geral:

* gerencia os consultórios;
* aplica a estratégia de alocação;
* e executa o algoritmo principal.

---

### O que mudaria usando estruturas mais simples?

Se fossem usados apenas arrays/listas soltas:

```csharp
var morningAppointments = new List<Appointment>();
var afternoonAppointments = new List<Appointment>();
```

O código ficaria:

* menos seguro;
* mais espalhado;
* mais difícil de validar;
* e muito mais propenso a bugs.

As classes ajudam porque encapsulam regras importantes do sistema.

---

# Parte 2 — Estratégia Algorítmica

## 4. Descrição do Algoritmo

### Estratégia utilizada:

**First Fit Decreasing (FFD)**

O funcionamento é simples:

### Passo 1 — Ordenação

Os atendimentos são ordenados do maior para o menor.

Isso melhora bastante o aproveitamento dos horários.

---

### Passo 2 — Alocação

Para cada atendimento:

* procura-se o primeiro consultório com espaço;
* se houver espaço → adiciona;
* se não houver → cria um novo consultório.

---

### Exemplo

Atendimentos:

* 120 min
* 60 min
* 10 min

Ordenação:

* 120
* 60
* 10

Resultado:

| Atendimento | Resultado           |
| ----------- | ------------------- |
| 120         | Consultório 1 manhã |
| 60          | Consultório 1 manhã |
| 10          | Consultório 1 manhã |

---

## 5. Tipo de Abordagem

### Abordagem Gulosa (Greedy)

Foi utilizada uma estratégia gulosa com heurística FFD.

O motivo principal:

* encontrar soluções boas rapidamente;
* sem custo computacional absurdo.

---

### Por que não força bruta?

Porque seria inviável.

Testar todas as combinações possíveis teria custo extremamente alto:

* crescimento fatorial;
* explosão combinatória;
* inviável para grandes quantidades de atendimentos.

---

### Por que FFD?

Porque:

* é simples;
* eficiente;
* muito usado em problemas de bin packing;
* e reduz desperdício de espaço.

Ordenar do maior para o menor evita fragmentação.

---

### Alternativas consideradas

### Best Fit

Tentaria encaixar no consultório “mais cheio”.

Problema:

* maior complexidade;
* e normalmente não melhora tanto na prática.

---

### Backtracking

Encontraria solução ótima.

Problema:

* extremamente lento;
* inviável para entrada grande.

---

### Programação Dinâmica

Seria mais precisa.

Problema:

* implementação extremamente complexa;
* e custo de memória alto.

---

## 6. Casos Onde Não Garante Solução Ótima

O FFD é heurístico.

Isso significa:

* normalmente produz soluções muito boas;
* mas não garante a solução perfeita em todos os casos.

Dependendo da combinação dos tempos:

* podem existir distribuições melhores.

Mesmo assim, na prática, ele costuma ter desempenho excelente.

---

## 7. Complexidade de Tempo

### Complexidade aproximada

[
O(n \log n + n \times m)
]

Onde:

* `n` = quantidade de atendimentos;
* `m` = quantidade de consultórios.

---

### Custos principais

### Ordenação

[
O(n \log n)
]

Por causa do `OrderByDescending`.

---

### Alocação

[
O(n \times m)
]

Porque:

* cada atendimento pode percorrer vários consultórios até encontrar espaço.

---

### Pior caso

Se cada atendimento precisar abrir um novo consultório:

[
O(n^2)
]

---

# Parte 3 — Decisões de Implementação

## 8. Como o Programa Decide Abrir Novos Consultórios

Foi utilizada estratégia de:

### Lazy Initialization

Ou seja:

* só abre um consultório novo quando realmente precisa.

---

### Funcionamento

```csharp
var office = FindBestOffice(appointment) ?? AddOffice();
```

O sistema:

1. procura espaço;
2. se não encontrar;
3. cria um novo consultório.

---

### Vantagens

* evita desperdício;
* minimiza consultórios;
* mantém lógica simples;
* e funciona muito bem com FFD.

---

## 9. Tratamento dos Atendimentos Expressos

Os atendimentos “expresso” são tratados como:

* atendimentos normais de 10 minutos.

```csharp
Duration = IsExpressed ? 10 : int.Parse(...);
```

---

### Por que isso?

Porque simplifica bastante o algoritmo.

Ao invés de criar regras especiais:

* o sistema apenas considera a duração real.

Isso:

* reduz complexidade;
* evita fragmentação;
* e mantém consistência.

---

## 10. Trechos Mais Inteligentes e Possíveis Melhorias

### Parte interessante

```csharp
private string GetTimeString(int minutes)
{
    var totalMinutes = StartHour * 60 + StartMinute + minutes;
    var hours = totalMinutes / 60;
    var mins = totalMinutes % 60;

    return $"{hours:D2}:{mins:D2}";
}
```

Essa função:

* converte minutos em horário;
* formata corretamente;
* e centraliza toda lógica temporal.

---

### Melhorias possíveis

A parte de agendamento ainda poderia:

* validar entradas inválidas;
* permitir múltiplas estratégias;
* registrar falhas;
* adicionar logs;
* e suportar configurações futuras.

---

# Resumo Executivo

* O problema foi modelado como um caso de **Bin Packing com múltiplas restrições**.
* A estratégia utilizada foi **First Fit Decreasing (FFD)**.
* O algoritmo é guloso, rápido e eficiente para cenários reais.
* As classes ajudam a manter organização, segurança e encapsulamento.
* O sistema abre consultórios apenas quando necessário.
* Atendimentos expressos são tratados como duração de 10 minutos.
* A complexidade típica é próxima de:

[
O(n \log n)
]

* O algoritmo não garante solução perfeita em todos os casos, mas apresenta excelente desempenho prático.
