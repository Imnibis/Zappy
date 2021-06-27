/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** action
*/

#include "server.h"

void do_actions(server_t *s, map_t *m)
{
    if (s->acs->dur == LOOK) {
        look(s, m, NULL);
        go_previous(s);
    }
}

void actions(server_t *s, map_t *m)
{
    clock_t new;
    action_t *head = s->acs;
    action_t *tmp = NULL;


    for (; s->acs != NULL; s->acs = s->acs->next) {
        new = clock() - s->acs->begin;
        if (new / CLOCKS_PER_SEC >= get_durations(s->acs->dur) / s->freq) {
            if (s->acs->dur == LOOK) {
                // do_actions(s, m);
                printf("LOOK on pos %d\n", s->acs->player->fd);
                if (s->acs->next == NULL) {
                    s->acs->prev->next = NULL;
                    free(s->acs);
                } else {
                    s->acs->prev->next = s->acs->next;
                    s->acs->next->prev = s->acs->prev;
                    free(s->acs);
                }
            }
        }
    }
    s->acs = head;
    // for (; s->acs->prev != NULL; s->acs = s->acs->prev);
}