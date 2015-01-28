#pragma once
#include <SFML/System.hpp>
#include <SFML/Graphics.hpp>
#include "PlayerClass.h"
#include "OgreClass.h"
#include "BoarClass.h"
#include "CrabClass.h"
#include "FairyClass.h"
#include "MageClass.h"
#include "PowerUp.h"
#include <list>
#include <mutex>
#include "SoundManager.h"

enum MenuState{MAIN,NUMBER_OF_PLAYERS};


class Game
{
protected:
	std::mutex Monster_mutex;
	std::mutex Attack_mutex;
	std::mutex PowerUp_mutex;

	sf::Vector2f screenSize;
	//Gamestate control
	GameState gamestate;
	MenuState menustate;
	
	

	//entity containers
	static const int MaxPlayerCount=2;
	int PlayerCount;
	PlayerClass Players[MaxPlayerCount];

	sf::Texture DragonTex;

	static const int MaxMonsters=100;
	
	std::list<Monster*> MonsterList;
	std::list<PowerUp*> PowerUpList;
	//display
	sf::RenderWindow window;

	//Audio
	SoundManager soundManager;
	//Text
	sf::Font fonts[1];
	static const int ARIAL=0;

	sf::Sprite Background;
	sf::Texture MenuTex[15];
	sf::Texture GamePlayBackgroundTex;
	sf::Texture LoseScreenTex;
	sf::Texture SpriteSheet;


	//Difficulty
	float TimeSinceLastSpawn;
	static const int SpawnDelay=2;//this is the default spawn time wait
	float TimeTilNextSpawn;//this is figured after each spawn based on the const spawn wait time and difficulty rate

	//Menu
	sf::IntRect Buttons[8];
	static const int bPlayGame=0;
	static const int bOptions=1;
	static const int bHighscore=2;
	static const int bInstructions=3;
	static const int bCreditz=4;
	static const int bOnePlayer=5;
	static const int bTwoPlayer=6;
	static const int bBack=7;
	//HUD
	sf::Sprite HUD;
	sf::Texture HUDtex[MaxPlayerCount];
	int Lives[2];
	sf::Text ScoreText[2];
	sf::Text BombText[2];
	sf::Text LivesText[2];
	
	float Scores[2];

	//lose scrren
	float DeadWait;
	float DeadDelay;

	//private functions
	void SetUpgame(int numPlayers);
	void ResetGame();
	void SpawnMonsters(sf::Time DeltaTime);//update's spawn
	void SpawnMonsters();//reset game spawn

	void HandleEvents();
	//update states
	void UpdateMenu(sf::Time DeltaTime);
	void UpdatePause(sf::Time DeltaTime);
	void UpdateGamePlay(sf::Time DeltaTime);
	void UpdateLoseScreen(sf::Time DeltaTime);

	//draw states
	void DrawMenu();
	void DrawPause();
	void DrawGamePlay();
	void DrawLoseScreen();

	void DrawHUD();
	void MakeButton(int fontNumber,std::string ButtonName,int ButtonNumber);
	float ClickDelay;
	float ClickWait;
	bool CanClick;

	int FramesSinceLastCollisionCheck;
	static const int FramesBetweenCollisionCheck=3;
	bool CheckButtonClick(int ButtonNumber);
	bool CheckButtonClick(int ButtonNumber,int fontNumber, std::string floatingMessage);
	void CollisionCheck();

	void PlayerDeath(int PlayerNum);
	void RewardPlayer(int PlayerNum, float score,sf::Vector2f startingPosition);


	void DebugMouse();

public:
	Game();
	~Game();
	void Update(sf::Time DeltaTime);
	void Draw();
	bool isOpen();
	
};


