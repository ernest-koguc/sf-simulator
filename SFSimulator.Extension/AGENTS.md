# Agent instructions

- Never write code comments unless the user explicitly requests them.
- Do not add ARIA attributes, including `aria-label`, or other accessibility-only markup.
- Don't start the dev server - assume it is arleady started when developing new things
- Use the local `parser-reference/` implementation as the source of truth for parsing the game. Check its response keys, field offsets, and update semantics before adding or changing game parsers.
