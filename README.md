# C# Rotozoomer source
=======================
First released (in this form) on: 25-dec-2005

'Rotozoomer' is an old demoscene effect. It was first invented by Chaos / Sanity on the Amiga 500 in 1989. I 
programmed my first rotozoomer in 1990 also on the Amiga 500 in MC68000 assembler for our 
Somewhere In Holland 1 democompetition winner 'Internal Exile' (for Perseus). Video of that demo can be found here:
https://www.youtube.com/watch?v=Xa89u44hK9c. If you want to run it on your own Amiga or WinUAE on PC, go here:
http://www.pouet.net/prod.php?which=12439

Because this implementation can eat any size of image and also can render to any size of canvas, the
clamping per pixel is rather slow (modulo's) instead of bit-voodoo. If you use a render canvas size which 
is of power of 2 and the image is also of a size of power of 2, you can clamp using bit operations, which
is much faster, but hey, it's the idea that counts ;)

To get that demoscene feeling, I've included one of my fake-drawings I made for one of the pixel competitions
years and years ago. 10 points for the first person who recognizes the original person :). The picture included 
is from 2001 and is called 'Fur'. 

Have fun!
