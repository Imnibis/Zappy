##
## EPITECH PROJECT, 2021
## zappy
## File description:
## Makefile
##

all:
	@make --no-print-directory -C server
clean:
	@make clean --no-print-directory -C server

fclean:
	@make fclean --no-print-directory -C server

re:
	@make re --no-print-directory -C server

server:
	@make --no-print-directory -C server

ai:

gui:
