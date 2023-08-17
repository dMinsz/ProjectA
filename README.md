# ProjectA

# Dependency

need to Maria DB or DB system for Login , Sign up Page

and need to Photon Cloud server setup or Photon server (PUN2)

# Sever Setup

Foldering

## In Project Folder
![InProject](./READMEImages/ServerData1.png)

## In Build Folder
![InBuild](./READMEImages/ServerData2.png)

## Server Json File Set
![Server Json](./READMEImages/JsonSetUp.png)

## If You Want Custom

You can Custom Source of This Link https://github.com/dMinsz/ProjectA/blob/master/Assets/Network/Scripts/DBManager.cs

# Game Play

## Login & SignUp

![Login](./READMEImages/Login.png)

![SignUp](./READMEImages/SignUp.png)

## Match Making

You can matchmake by logging in.

1vs1 , 2vs2, 3vs3

![MatchMaking](./READMEImages/MatchMaking.png)

## Choose Character (In Room)

After the matchmaking, the character selection scene will appear.

You get to choose a character with other users.

If everyone is ready
The host can start the game.

![CharacterChoose](./READMEImages/CharacterChoose.png)

Host Can Start

![Host](./READMEImages/Host.png)

Ohter Player Waitng

![Remote](./READMEImages/Remote.png)

# In Game

## Count Down

When you start the game, it waits for all players to enter the game scene, then the game starts 5 seconds later.

![GameStart](./READMEImages/GameStart.png)

## Player Attack and Use Skill

![SkillRange](./READMEImages/SkillRange.png)

![SkillEffect](./READMEImages/SkillEffect.png)

If there is a user from another team within the range of the skill

Can Damage

## Play and EndGame

![PuckMove](./READMEImages/PuckMove.png)


The game is over when one team reaches 5 points or when the allotted time runs out.

![GameEnd](./READMEImages/GameEnd.png)

# Delay compensation

We did not use Photon RigidBody,TransForm View to compensate for server latencies.

You can See this Folder => https://github.com/dMinsz/ProjectA/tree/master/Assets/Network/Scripts/optimization

and Ball Move CCD => https://github.com/dMinsz/ProjectA/blob/master/Assets/Game/Scripts/Puck.cs
