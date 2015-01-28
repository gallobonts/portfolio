#include "LinkerFile.h"
#pragma once;


//threads
void RenderThread(Game* Game);
//global functions
const int ENTITYLENGTH=1;
Game game;

int main()
{
	

//game loop
	sf::Thread thread(&RenderThread,&game);//create and start our renderthread
    thread.launch();

	sf::Clock clock;//clock used to time the game's events
	GameState gamestate= GAMEPLAY;



    while (game.isOpen())
    {	
						sf::Time DeltaTime =clock.restart();//finds the time elapsed since last call
						game.Update(DeltaTime);
    }

    return 0;
}


void RenderThread(Game* game)
{

	while(game->isOpen())
	{
		game->Draw();
	}

}