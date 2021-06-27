##
## EPITECH PROJECT, 2021
## zappy
## File description:
## Makefile
##

all:
	server
	ai
	gui

clean:
	@make clean --no-print-directory -C server
	@rm zappy_ai

fclean: clean
	@make fclean --no-print-directory -C server

re:
	fclean
	ai
	gui
	@make re --no-print-directory -C server

server:
	@make --no-print-directory -C server

ai:
	@cp ai/zappy_ai ./
	@chmod +x zappy_ai

gui:
	@cp client/zappy_gui ./
	@chmod +x zappy_gui

.PHONY: all clean fclean re server ai gui