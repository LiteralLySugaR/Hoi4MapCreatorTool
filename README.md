# Map Creator Tool Documentation
This is (not the most) detailed documentation of this tool, explaining how to use it and what does what.
# Table of Content
- Main[^1]
- State History Entry[^2]
- Use Array[^3]
- Resources Entry[^4]
- Province Definition[^5]

#[^1]: 1.0 Main | State Manipulation Tool
The very first thing you see and the base for everything in there. From here you can go into other manipulation tools like **Province Manipulation Tool**, **Resources Entry Tool** **History Entry Tool**.
### Supported Commands

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

Fourth Manilupation Tool only goal which is to perform manipulations with state's resources.
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

Third Manipulation Tool which goal is to perform manipulations with an array of states history entry at the same time.
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

#[^2]: 2.0 State History Entry | History Entry Tool
The second manipulation tool that is base for every changes in state's history entry.
### Supported Commands

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

#[^3]: 3.0 Use Array | History Entry for Multiple States Tool
Third Manipulation Tool or rather part 2 of second Tool. Allows to perform most manipulations from **State History Entry** but for multiple states at once.
Uses same classes as **State History Entry**.
### Supported Commands
It uses the same Commands as in **State History Entry** with `victory_points` and `resourcesentry/resent` excluded and one new command:

- showarray

Will show every states files you selected.

#[^4]: 4.0 Resources Entry | Resources Entry Tool
Fourth Manipulation Tool that allows you to change resources entry. Can be accessed both from **Main** or **State History Entry**.
### Supported Commands

- help \<resources>

Same as `help` command anywhere, will show this list without detailed explanations. "resources" is optional (or, again, situational) and will show all available resources.

`resources` output:
```
0 - aluminium
1 - chromium
2 - oil
3 - rubber
4 - tungsten
5 - steel
```
- edit [entry] [value]

Will edit `entry` replacing old value with `value`.
- end

Console App will be closed.
- clear

Will clear the console.
- create

Will create new resources entry.
- check

Checks if there is a resource entry, returns `true` or `false`.
- add [entry] [value]

Will add resource entry with value. `[entry] = [value]`.
- remove [entry]

Will remove entry if it exists.
- return

Will return to **State History Entry**.
- returnmain

Will return to **Main**.
## 4.1 Resources Entry | Resources Edit Entry class

Used only by `edit` command and it edit the entry with a new value.
## 4.2 Resources Entry | Resources Add Entry class

Used only by `add` command and it adds a new entry with the value.
## 4.3 Resources Entry | Resources Remove Entry class

Used only by `remove` command and it removes the entry.
## 4.4 Resources Entry | Create Resource Entry class

Used only by `create` command and it creates a new resource entry if there is none.
```
resources = {
}
```
## 4.5 Resources Entry | Check Resources Entry class

Used only by `check` command and it check if there is resources entry. Returns `true` or `false`.
#[^5]: 5.0 Province Definition | Province Manipulation Tool
Fifth Manipulation Tool that alows you to perform manipulations with provinces.

At the moment (1.6.1) you can only use one command related to that tool.
### Supported Commands

- help \<createLandType/clt>

`help` command at this point i think doesnt need to be presented. "createLandType/clt" is optional but required to understand what this command does.

`createLandType/clt` output:

This command requires province map (map/provinces.bmp), a Terrain Input File path and output file name. Optional you can set starting pixel to check from and last pixel position to check \(\"-\" BETWEEN X AND Y IS REQUIRED), otherwise it will take longer to check since it will check EACH pixel. The Output file will ALWAYS be created as a .txt file.
- clear

This will clear the console.
- end

Console App will be closed.
- return

Will return to **Main**.
- createLandType/clt [TerrainInput] [outputFileName] <MinX-MinY> <MaxX-MaxY>

Change every land province entry according to the Terrain Input.

Example: `clt map/MyTerrainInput.bmp MyDefinition 210-200 500-500`

## 5.1 Province Definition | Terrain Input

Terrain Input file can be any extension until its an image. (.bmp .png .jpg .jpeg ...).

Terrain Input should have a specific colour pallet:

![colours](https://user-images.githubusercontent.com/75783315/225564146-a900ff13-9d27-4db2-8ff1-460d71d1fb3b.png)

And using provinces.bmp in photoshop put it below Terrain Input and (with it selected) start selecting province that you want to change category for, using paint bucket tool change colour of the selected areas on Terrain Input repeat for every categories you want to add/change and you'll have something like this (Used my Tsushima Map for TsushimaWars mod):

![Terrain Input](https://user-images.githubusercontent.com/75783315/225566269-c045ccde-14aa-4f16-beae-b5e2875b9f28.png)

When done, save the Terrain Input inside your mod folder.

## 5.2 Province Definition | Read Pixel From Provinces class

Main class for changing `Definition.csv` and exists in 3 variants:
- ReadPixelFromProvinces(string TerrainInputFile)
- ReadPixelFromProvinces(string TerrainInputFile, string MinXY, string MaxXY)
- ReadPixelFromProvinces(string TerrainInputFile, string MinXY)

It compares colours on Terrain Input with provinces, it reads the province only when the same pixel colour on Terrain Input is not water.
Once the province was found not in water colour, it saves XY position of the pixel, its RGB colour, Type to set and its ID.
## 5.3 Province Definition | Replace Province Type Entry class

Last step of creating entries to-copy-in `Definition.csv` and it replaces entries according to the information given from **ReadPixelFromProvinces** inside a new file.
## 5.4 Province Definition | Find Province Id By Color class

Used by **ReadPixelFromProvinces** and it finds province ID in `Definition.csv` by given colour.
## 5.5 Province Definition | Get Color From String RGB class

Converts (for example) "255 0 127" into Color.
## 5.6 Province Definition | Convert Color To Type class

Converts Color to Province Type. Will return "plains" if nothing else matched.
