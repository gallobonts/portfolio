#pragma once
#include "Game.h"
#include <SFML\Window.hpp>

Game::Game()
{
	//state control
	gamestate= MENU;
	menustate= MAIN;

	//graphics
	window.setActive(false);
	window.create(sf::VideoMode(800, 600), "Dragon Survival");//we use renderwindow instead of window to put the SFML's graphics module into use rather than use OpenGL
	window.setActive(false);//deactivate opengl context...appears white screen without
	window.setFramerateLimit(45); //our game's frame rate
	
	//screen size
	screenSize= sf::Vector2f(window.getSize());
	
	//setup Players
	Players[0]=  PlayerClass(screenSize,1); 
	Players[1]=  PlayerClass(screenSize,2); 

	//setup Start Monsters
	MonsterList.push_back(new OgreClass(sf::Vector2f(screenSize.x/2,screenSize.y/2),screenSize));
	
	//fonts
	fonts[ARIAL].loadFromFile("Assets/Fonts/arial.ttf");

	TimeSinceLastSpawn=0;
	TimeTilNextSpawn=SpawnDelay;
	
	//spritesheet
	SpriteSheet.loadFromFile("Assets/Textures/SpriteSheet.png");

	//Lose Screen
	DeadWait=0;
	DeadDelay=3.0f;

	//HUD
	HUDtex[0].loadFromFile("Assets/Textures/HUD_1P.png");
	HUDtex[1].loadFromFile("Assets/Textures/HUD_2P.png");

	HUD.setTexture(HUDtex[0]);

	HUD.setOrigin(HUD.getLocalBounds().width/2,HUD.getLocalBounds().height/2);
	HUD.setPosition(screenSize.x/2,screenSize.y/2);
	HUD.setScale(screenSize.x/640,screenSize.y/480);

	
	ScoreText[0].setPosition(130,15);
	ScoreText[0].setCharacterSize(25);
	ScoreText[0].setOrigin(0,0);
	ScoreText[0].setFont(fonts[ARIAL]);
	ScoreText[0].setColor(sf::Color::White);

	ScoreText[1].setPosition(566,15);
	ScoreText[1].setCharacterSize(25);
	ScoreText[1].setOrigin(0,0);
	ScoreText[1].setFont(fonts[ARIAL]);
	ScoreText[1].setColor(sf::Color::White);

	LivesText[0].setPosition(92,530);
	LivesText[0].setCharacterSize(40);
	LivesText[0].setOrigin(0,0);
	LivesText[0].setFont(fonts[ARIAL]);
	LivesText[0].setColor(sf::Color::White);

	LivesText[1].setPosition(525,530);
	LivesText[1].setCharacterSize(40);
	LivesText[1].setOrigin(0,0);
	LivesText[1].setFont(fonts[ARIAL]);
	LivesText[1].setColor(sf::Color::White);

	
	BombText[0].setCharacterSize(40);
	BombText[0].setOrigin(0,0);
	BombText[0].setFont(fonts[ARIAL]);
	BombText[0].setColor(sf::Color::White);

	BombText[1].setPosition(750,530);
	BombText[1].setCharacterSize(40);
	BombText[1].setOrigin(0,0);
	BombText[1].setFont(fonts[ARIAL]);
	BombText[1].setColor(sf::Color::White);


	//Background
	GamePlayBackgroundTex.loadFromFile("Assets/Textures/Plains.bmp");

	MenuTex[0].loadFromFile("Assets/Textures/MenuScreens/Menu.png");
	MenuTex[1].loadFromFile("Assets/Textures/MenuScreens/Menu_Play Game.png");
	MenuTex[2].loadFromFile("Assets/Textures/MenuScreens/Menu_Options.png");
	MenuTex[3].loadFromFile("Assets/Textures/MenuScreens/Menu_HighScores.png");
	MenuTex[4].loadFromFile("Assets/Textures/MenuScreens/Menu_Instructions.png");
	MenuTex[5].loadFromFile("Assets/Textures/MenuScreens/Menu_Credits.png");
	MenuTex[6].loadFromFile("Assets/Textures/MenuScreens/PlayGame.png");
	MenuTex[7].loadFromFile("Assets/Textures/MenuScreens/Options1.png");
	MenuTex[8].loadFromFile("Assets/Textures/MenuScreens/Options2.png");
	MenuTex[9].loadFromFile("Assets/Textures/MenuScreens/Options3.png");
	MenuTex[10].loadFromFile("Assets/Textures/MenuScreens/Options4.png");
	MenuTex[11].loadFromFile("Assets/Textures/MenuScreens/Options5.png");
	MenuTex[12].loadFromFile("Assets/Textures/MenuScreens/HighScores.png");
	MenuTex[13].loadFromFile("Assets/Textures/MenuScreens/Controls.png");
	MenuTex[14].loadFromFile("Assets/Textures/MenuScreens/Credits.png");

	LoseScreenTex.loadFromFile("Assets/Textures/LoseScreen.png");

	Background.setTexture(MenuTex[0]);
	Background.setPosition(screenSize.x/2,screenSize.y/2);
	Background.setOrigin(Background.getLocalBounds().width/2,Background.getLocalBounds().height/2);
	Background.setScale(screenSize.x/650,screenSize.y/480);

	// Menu
	
	Buttons[bPlayGame].left=310;
	Buttons[bPlayGame].top=240;
	Buttons[bPlayGame].width=145;
	Buttons[bPlayGame].height=20;

	
	Buttons[bOptions].left=320;
	Buttons[bOptions].top=280;
	Buttons[bOptions].width=125;
	Buttons[bOptions].height=20;

	Buttons[bHighscore].left=295;
	Buttons[bHighscore].top=315;
	Buttons[bHighscore].width=180;
	Buttons[bHighscore].height=20;

	Buttons[bInstructions].left=275;
	Buttons[bInstructions].top=250;
	Buttons[bInstructions].width=210;
	Buttons[bInstructions].height=10;


	Buttons[bCreditz].left=325;
	Buttons[bCreditz].top=390;
	Buttons[bCreditz].width=115;
	Buttons[bCreditz].height=20;
	
	
	Buttons[bOnePlayer].left=315;
	Buttons[bOnePlayer].top=245;
	Buttons[bOnePlayer].width=150;
	Buttons[bOnePlayer].height=45;

	Buttons[bTwoPlayer].left=315;
	Buttons[bTwoPlayer].top=330;
	Buttons[bTwoPlayer].width=155;
	Buttons[bTwoPlayer].height=35;

	Buttons[bBack].left=350;
	Buttons[bBack].top=515;
	Buttons[bBack].width=425;
	Buttons[bBack].height=555;

	CanClick=true;
	ClickWait=0;
	ClickDelay=.4f;

	FramesSinceLastCollisionCheck=0;
	soundManager.Play(Sound::MenuLoop);
}


void Game::SetUpgame(int numPlayers)
{
	//state control
	gamestate= GAMEPLAY;
	menustate= MAIN;
	Background.setTexture(GamePlayBackgroundTex);
    soundManager.Play(Sound::GameLoop);
	PlayerCount=numPlayers;
	Lives[0]=3;
	Scores[0]=0;
	Scores[1]=0;
	Players[0].SetLife(true);
	Players[0].ResetPosition();
	BombText[0].setPosition(750,530);

	HUD.setTexture(HUDtex[PlayerCount-1]);

	if(PlayerCount==1)
	{
		Lives[1]=0;

	}
	else
	{
		Lives[1]=3;
		Players[1].SetLife(true);
		BombText[0].setPosition(319,530);
		Players[1].ResetPosition();
	}

	MonsterList.clear();
	PowerUpList.clear();
}
void Game::Update(sf::Time DeltaTime)
{
	HandleEvents();


	switch(gamestate)
	{
	case MENU:
		UpdateMenu(DeltaTime);
		break;
	case GAMEPLAY:
			UpdateGamePlay(DeltaTime);
			break;
	case LOSESCREEN:
		UpdateLoseScreen(DeltaTime);
	default:
			{}
	}

	

}

void Game::UpdateLoseScreen(sf::Time DeltaTime)
{
		DeadWait+=DeltaTime.asSeconds();
		if(DeadWait>=DeadDelay)
		{
			gamestate=GameState::MENU;
			Background.setTexture(MenuTex[0]);
			menustate=MenuState::MAIN;
			soundManager.Play(Sound::Music::MenuLoop);
			DeadWait=0;
		}
}

void Game::UpdateMenu(sf::Time DeltaTime)
{
	if(CanClick)
	{
		switch(menustate)
		{
		case MAIN:
			if(CheckButtonClick(bPlayGame))
			{menustate=NUMBER_OF_PLAYERS;Background.setTexture(MenuTex[6]);CanClick=false;}
			break;
	
		case NUMBER_OF_PLAYERS:
			if(CheckButtonClick(bOnePlayer)&&CanClick)
			{SetUpgame(1);CanClick=false;}
			else if(CheckButtonClick(bTwoPlayer))
			{SetUpgame(2);CanClick=false;}
			else if(CheckButtonClick(bBack))
			{menustate=MAIN;Background.setTexture(MenuTex[0]);CanClick=false;}
			break;
		}
	}//end if can click

	else if(!CanClick)
	{
		ClickWait+=DeltaTime.asSeconds();
		if(ClickWait>=ClickDelay)
		{
			CanClick=true;
			ClickWait=0;
		}

	}
}

bool Game::CheckButtonClick(int ButtonNumber)
{

	if (sf::Mouse::isButtonPressed(sf::Mouse::Left))
	{
		sf::Vector2i localPosition = sf::Mouse::getPosition(window);
		return(Buttons[ButtonNumber].contains(localPosition));
	}
	else
	{return false;}
	
}
bool Game::CheckButtonClick(int ButtonNumber,int fontNumber, std::string floatingMessage)//doesnt work
{
	ButtonNumber-=1;
	sf::Vector2i localPosition = sf::Mouse::getPosition(window);
	sf::Text text;

	text.setFont(fonts[fontNumber]);
	text.setString(floatingMessage);
	text.setCharacterSize(Buttons[ButtonNumber].height/3);
	text.setOrigin(text.getLocalBounds().width/2,text.getLocalBounds().height/2);
	text.setColor(sf::Color::Green);
	

	if (Buttons[ButtonNumber].contains(localPosition))
	{
		text.setPosition((float)localPosition.x,(float)localPosition.y-10);
		window.draw(text);
		return sf::Mouse::isButtonPressed(sf::Mouse::Left);
	}
	else
	{return false;}
}

void Game::UpdateGamePlay(sf::Time DeltaTime)
{	
	UpdateReturn Ureturn;
	Attack_mutex.lock();
	for(int i=0;i<PlayerCount;i++)
	{Ureturn=Players[i].Update(DeltaTime);soundManager.Play(Ureturn.SFX);}
	Attack_mutex.unlock();

	
	if(!MonsterList.empty())
	{
	for(std::list<Monster*>::const_iterator i=MonsterList.begin(),end=MonsterList.end();i!=end;i++)
	{(*i)->Update(DeltaTime);}
	}

	if(!PowerUpList.empty())
	{
	for(std::list<PowerUp*>::const_iterator i=PowerUpList.begin(),end=PowerUpList.end();i!=end;i++)
	{(*i)->Update(DeltaTime);}
	}

	FramesSinceLastCollisionCheck++;
	if(FramesSinceLastCollisionCheck>=FramesBetweenCollisionCheck)
	{
		CollisionCheck();
		FramesSinceLastCollisionCheck=0;
	}


	SpawnMonsters(DeltaTime);
	
}

void Game::SpawnMonsters()
{
	int RandLocation= rand()%4;
	sf::Vector2f NewPosition;
	switch(RandLocation)
	{
	case 0:
		NewPosition=sf::Vector2f(50,300);
		break;
	case 1:
		NewPosition=sf::Vector2f(400,50);
		break;
	case 2:
		NewPosition=sf::Vector2f(750,300);
		break;
	case 3:
		NewPosition=sf::Vector2f(400,550);
		break;
	}

		int RandMonst= rand()%5;
		switch (RandMonst)
		{
		case 0:
			MonsterList.push_back(new OgreClass(NewPosition,screenSize));
		break;
		case 1:
			MonsterList.push_back(new BoarClass(NewPosition,screenSize));
			break;
		case 2:
			MonsterList.push_back(new FairyClass(NewPosition,screenSize));
			break;
		case 3:
			MonsterList.push_back(new CrabClass(NewPosition,screenSize));
			break;
		case 4:
			MonsterList.push_back(new MageClass(NewPosition,screenSize));
			break;
		}

		TimeSinceLastSpawn=0;
		TimeTilNextSpawn=SpawnDelay;

}
void Game::SpawnMonsters(sf::Time DeltaTime)
{
	if(MonsterList.size()<MaxMonsters)
	{
	TimeSinceLastSpawn+=DeltaTime.asSeconds();
	if(TimeSinceLastSpawn>=TimeTilNextSpawn)
	{
			
		int RandLocation= rand()%4;
		sf::Vector2f NewPosition;
		switch(RandLocation)
		{
		case 0:
			NewPosition=sf::Vector2f(50,300);
			break;
		case 1:
			NewPosition=sf::Vector2f(400,50);
			break;
		case 2:
			NewPosition=sf::Vector2f(750,300);
			break;
		case 3:
			NewPosition=sf::Vector2f(400,550);
			break;
		}
	
			int RandMonst= rand()%5;
			switch (RandMonst)
			{
			case 0:
				MonsterList.push_back(new OgreClass(NewPosition,screenSize));
				break;
			case 1:
				MonsterList.push_back(new BoarClass(NewPosition,screenSize));
				break;
			case 2:
				MonsterList.push_back(new FairyClass(NewPosition,screenSize));
				break;
			case 3:
				MonsterList.push_back(new CrabClass(NewPosition,screenSize));
				break;
			case 4:
				MonsterList.push_back(new MageClass(NewPosition,screenSize));
				break;
			}

			TimeSinceLastSpawn=0;
			TimeTilNextSpawn*=.99f;
	}

		
	}
}
void Game::CollisionCheck()
{
	std::vector<sf::FloatRect> PossibleCollisions;
	Sound::SoundEffect sfx;

	PowerUp* used;
	for(std::list<PowerUp*>::const_iterator i = PowerUpList.begin(), end=PowerUpList.end();i!=end;i++)
	{
		for(int p=0;p<PlayerCount;p++)
		{
			CollisionID collisionID=Players[p].CheckCollisions((*i)->GetCollision());
			if(collisionID==e_Player)
			{
				Players[p].PowerUp((*i)->GetType());
				if((*i)->GetType()==PowerUpType::eLives)
				{Lives[p]++;}
				used=(*i);
			}
		}
	}

	PowerUp_mutex.lock();
	PowerUpList.remove(used);
	PowerUp_mutex.unlock();

	if(!MonsterList.empty())
	{
	Monster* dead;

	for(std::list<Monster*>::const_iterator i=MonsterList.begin(),end=MonsterList.end();i!=end;i++)
	{

		
			PossibleCollisions=(*i)->GetCollision();
			for(unsigned int colliderNum=0;colliderNum<PossibleCollisions.size();colliderNum++)
			{
				
				for(int k=0;k<PlayerCount;k++)//playercount...
				{
					CollisionID collisionID=Players[k].CheckCollisions(PossibleCollisions[colliderNum]);
					if(collisionID != CollisionID::e_Fire)
					{
					//	i++;
					
					}
					switch(collisionID)
					{
					case e_Player:
						PlayerDeath(k);
						break;
					case e_Fire:
						sf::Vector2f StartPos= sf::Vector2f(PossibleCollisions[colliderNum].left+PossibleCollisions[colliderNum].width/2,PossibleCollisions[colliderNum].top+PossibleCollisions[colliderNum].height/2);
						RewardPlayer(k,(*i)->HandleCollision(colliderNum,sfx),StartPos);
						soundManager.Play(sfx);
						Monster_mutex.lock();
						//i=MonsterList.erase(i);
						dead=(*i);
						Monster_mutex.unlock();

						break;
					}	
				}//end playercount loop
			}//end for possible collisions loop
	}//end for monstercount loop
			Monster_mutex.lock();
			MonsterList.remove(dead);
			Monster_mutex.unlock();
	}
		
}

void Game::RewardPlayer(int PlayerNum, float score,sf::Vector2f startingPosition)
{
	Scores[PlayerNum]+=score;
	int ChancePowerUp=10;
	int randPowerUp= RandomNumber(0,100);
	if(randPowerUp<=ChancePowerUp)
	{
		PowerUp_mutex.lock();
		PowerUpList.push_back(new PowerUp(startingPosition));
		PowerUp_mutex.unlock();
	}
}

void Game::PlayerDeath(int PlayerNum)
{

	if(Lives[PlayerNum]>0)
	{Lives[PlayerNum]--;}

	if(Lives[0]+Lives[1]<=0)
	{gamestate=LOSESCREEN;Background.setTexture(LoseScreenTex);soundManager.Play(Sound::Music::LoseLoop);}
	else 
	{
		if(Lives[PlayerNum]<=0)
		{
			Players[PlayerNum].SetLife(false);
			
		}
	//	gamestate=PAUSE;
		ResetGame();
	
	}
	
}

void Game::ResetGame()
{

	Monster_mutex.lock();
//	MonsterList.clear();
	Monster_mutex.unlock();
}



void Game::HandleEvents()
{
	sf::Event event;
		
	//if the game is quit
	while (window.pollEvent(event))
    {
      if (event.type == sf::Event::Closed)
			{window.close();}
     }

}


void Game::Draw()
{
		
	//DebugMouse();
	//clear old screen
	window.clear(sf::Color::Black);
	window.draw(Background);
	//set up new screen
	switch(gamestate)
	{
	case MENU:
		DrawMenu();
		break;
	case PAUSE:
		DrawPause();
	case GAMEPLAY:
		DrawGamePlay();
		break;
	case LOSESCREEN:
		DrawLoseScreen();
		break;
	default:
		{}
	}//end gamstate switch
	
	//draw new screen
	window.display();
}

void Game::DrawMenu()
{
	
	sf::RectangleShape rectangle;
	switch(menustate)
	{
	case MAIN:
		//Play Game
//		MakeButton(ARIAL,"Play Game",1);
		//Options
//		MakeButton(ARIAL,"Options",2);
		//Score Board
//		MakeButton(ARIAL,"Score Board",3);
		//Credits
//		MakeButton(ARIAL,"Credits",4);
		//Instructions
//		MakeButton(ARIAL,"Instructions",5);
		break;
	case NUMBER_OF_PLAYERS:
		//1 Player
//		MakeButton(ARIAL,"1 Player",1);
		//2 Player
//		MakeButton(ARIAL,"2 Player",2);
		//Back
//		MakeButton(ARIAL,"Back",3);
		break;
	}

}

void Game::MakeButton(int fontNumber,std::string ButtonName,int ButtonNumber)
{
	ButtonNumber-=1;
	sf::Text text;
	text.setFont(fonts[fontNumber]);
	sf::RectangleShape rectangle;


	text.setString(ButtonName);
	text.setCharacterSize(Buttons[ButtonNumber].height-10);
	text.setOrigin(text.getLocalBounds().width/2,text.getLocalBounds().height/2);
	text.setPosition((float)Buttons[ButtonNumber].left+(float)(Buttons[ButtonNumber].width/2),(float)Buttons[ButtonNumber].top+(float)(Buttons[ButtonNumber].height/2)-10);
	text.setColor(sf::Color::Green);
	
	rectangle.setPosition((float)Buttons[ButtonNumber].left,(float)Buttons[ButtonNumber].top);
	rectangle.setSize(sf::Vector2f((float)Buttons[ButtonNumber].width,(float)Buttons[ButtonNumber].height));
	rectangle.setFillColor(sf::Color::White);

		
	window.draw(rectangle);
	window.draw(text);

}
void Game::DrawPause()
{
}

void Game::DrawGamePlay()
{


	DrawHUD();

	Attack_mutex.lock();
	for(int i=0;i<PlayerCount;i++)
	{
		Players[i].Draw(&window);
	}
	Attack_mutex.unlock();


	Monster_mutex.lock();
	if(!MonsterList.empty())
	{
	for(std::list<Monster*>::const_iterator i=MonsterList.begin(),end=MonsterList.end();i!=end;i++)
	{
		(*i)->Draw(&window);
	}
	}
	Monster_mutex.unlock();

	PowerUp_mutex.lock();
	if(!PowerUpList.empty())
	{
	for(std::list<PowerUp*>::const_iterator i=PowerUpList.begin(),end=PowerUpList.end();i!=end;i++)
	{
		(*i)->Draw(&window);
	}
	}
	PowerUp_mutex.unlock();


}
void Game::DrawHUD()
{
	window.draw(HUD);

	BombText[0].setString(std::to_string(Players[0].GetExplosiveFireBallCount()));
	window.draw(BombText[0]);
	
	LivesText[0].setString(std::to_string(Lives[0]));
	window.draw(LivesText[0]);

	ScoreText[0].setString(std::to_string((int)Scores[0]));
	window.draw(ScoreText[0]);


	if(PlayerCount==2)
	{
	
		BombText[1].setString(std::to_string(Players[1].GetExplosiveFireBallCount()));
		window.draw(BombText[1]);
	
		LivesText[1].setString(std::to_string(Lives[1]));
		window.draw(LivesText[1]);
	
		ScoreText[1].setString(std::to_string((int)Scores[1]));
		window.draw(ScoreText[1]);
	}
}

void Game::DrawLoseScreen()
{


}

void Game::DebugMouse()
{
	sf::Vector2i localPosition = sf::Mouse::getPosition(window);
	std::cout<<"Mouse Position = "+std::to_string(localPosition.x)+","+std::to_string(localPosition.y)<<std::endl;
}
bool Game::isOpen()
{
	return window.isOpen();
}

Game::~Game()
{
	
}