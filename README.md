# Map Creator Tool Documentation
This is (not the most) detailed documentation of this tool, explaining how to use it and what does what.

# 1.0 Main | State Manipulation Tool
The very first thing you see and the base for everything in there. From here you can go into other manipulation tools like **Province Manipulation Tool**, **Resources Entry Tool** **History Entry Tool**.
### Supported Commands (1.6.1)

> value in \"<>" means optional. (if the value is not "value" this means exact value to type)

> value in "[]" parenthesis means required.

- help \<categories>

Shows this list but without explanations of what does what. "categories" argument is optional (or better say situational) and will show every available state categories and their IDs.

`categories` output:
```
0 - wasteland
1 - enclave
2 - tiny_island
3 - pastoral
4 - small_island
5 - rural
6 - town
7 - large_town
8 - city
9 - large_city
10 - metropolis
11 - megapolis
```
- about

Small info on the tool: version and author.
- resourcesentry/resent [stateID]

Third Manilupation Tool only goal which is to perform manipulations with state's resources.
- category [stateID] [value]

Set state's category. If value is unknown, ID is out of range or string value is nowhere to be found, will use **Wasteland (id 0)**.
- historyentry/hisent [stateID]

Second Manipulation Tool which goal is to perform manipulations with state's history entry within state file. ` history = { [right here] } `
- provincedefinition/provdef

Fifth Manipulation Tool which goal is to perform manipulations with provinces mainly in `Definition.csv`. Also provides you with unique tool.
- manpower [stateID] [value]

Set state's manpower. any value acceptable but to avoid bugs in game highly recommended to use 32 bit integer value.
- transfer [stateID] [provinces]

Transfer "selected" provinces to the state.
Example: transfer 1 1 2 3 4 5 (provinces 1 2 3 4 5 will be transfered to state with id 1).
- usearray [statesIDS]

Fourth Manipulation Tool which goal is to perform manipulations with an array of states history entry at the same time.
- clear

Will clear everything in the console.
- end

Console App will be closed.
- create [Name] [provinces]

Create a state with chosen provinces, [Name] is used for localisation entry while the ID is defined by the amount of files inside **history/states/** + 1
Example: create NewState 1 2 3 4 5

## 1.1 Main | Create Script class

This class is used in state creation and is writing a blank state script.
```
state = {
  id = [ID]
  name = STATE_" + [ID]
  provinces = {
    1 2 3 4 5 
  }
  manpower = 0
  buildings_max_level_factor = 1.000
  local_supplies = 0.000
}
```
## 1.2 Main | Create Localisation Entry class

This class is used in state creation and is creating a localisation entry.
This class is very hardcoded and will always try to write in **localisation/english/state_names_l_english.yml**. `[STATE_ID]:0 "[NewState]" `
## 1.3 Main | Get State By ID class

Used by every commands and has `[StateID]` as an argument and it returns state path (**history/states/1-ThisState.txt**).
## 1.4 Main | Redact State Parameter class

Used by `category` and `manpower` commands to add or edit category or manpower entries.
## 1.5 Main | Redact State class

Used by `transfer` command and it takes all the chosen provinces from one state(s) to another.
## 1.6 Main | Create State class

Used by `create` command and it creates new file and using **Create Script class** mentioned in 1.2 Main.
## 1.7 Main | Get Free State class

Used by `create` command and it search for free state ID by simply checking amount of files inside **history/states/** and adding 1.
## 1.8 Main | Get Provinces in States class

Used by `create` and `transfer` commands and it returns state or list of states paths in which it found chosen provinces.
## 1.9 Main | Convert To String Category class

Used by `category` command and it converts Category ID to string value.

# 2.0 State History Entry | History Entry Tool
The second manipulation tool that is base for every changes in state's history entry.
### Supported Commands (1.6.1)

- help \<entries>

Same as `help` command anywhere, will show this list without detailed explanations. "entries" is optional (or again, situational) and will show all available entries.

`entries` output:
```
- victory_points [province] [value]
- owner [countryTAG]
- add_core_of [countryTAG]
- add_claim_by [countryTAG]
- set_demilitarized_zone [yes/no]
```
- resourcesentry

Access **Resources Entry Tool**.
- end

Console App will be closed.
- clear

Will clear everything in the console.
- create

Will create a history entry in state's file. Will never create if there is already one.
- check

Will check for history entry, returns true or false.
- setowner [countryTAG]

Set state's owner.
- add [entry] [value]

Add an entry. If there is already same entry with same value it will create another.
- remove [entry] <EntryValue>

Will remove an entry with **EntryValue**, otherwise it will remove all entries of that kind.
- return

Will return to Main.
## 2.1 State History Entry | State Add Entry class

Exists in two variations: One requires variables (string state, string entry, string value) while second requires (string state, string entry, string[] value), used only by `add` command, first and second value are **state** and **entry** while the last is the value(s) for the entry. Since **victory_points** entry has 2 values (provinceID and value), its used inside the `StateAddEntry(string, string, string[])`.
## 2.2 State History Entry | State Remove Entry class

Exists in two variations: One requires variables (string state, string entry) while second (string state, string entry, string value), used only by `remove` command and first one removes every entries of that kind while the second removes entry only with the same value.
## 2.3 State History Entry | State Set Owner class

Used only by `setowner` command and checks for the `owner` entry and change its value, otherwise create new entry.
## 2.4 State History Entry | Create History Entry class

Used only by `create` command and it creates a history entry if there is none, otherwise it wont.
```
  history = {
  }
```
## 2.5 State History Entry | Check History Entry class

Used only by `check` command and it check for existance of history entry inside state's file. Returns **true** or **false**.
