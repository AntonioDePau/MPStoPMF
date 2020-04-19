# MPStoPMF
Allows the "conversion" of MPS files to PMF (It's basically almost just adding a header) usually used as PSP videos.

This tool takes an MPS file and adds a header as follows:<br />
50 53 4D 46 30 30 30 34 <strong>00 00 08 00 00 B8 10 00</strong><br />
00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00<br />
00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00<br />
00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00<br />
00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00<br />
00 00 00 4E 00 00 00 01 5F 90 00 00 00 69 6F 75<br />
00 00 61 A8 00 01 5F 90 02 01 00 00 00 34 00 00<br />
00 01 5F 90 00 00 00 69 6F 75 00 01 00 00 00 22<br />
00 02 E0 00 21 EF 00 00 00 00 00 00 00 00 1E 11<br />
00 00 BD 00 20 04 00 00 00 00 00 00 00 00 00 00<br />
02 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00<br />
<br />
Where "30 30 30 34" represents the type of PMF file (0004).<br />
Where "00 B8 10 00" represents the length of the original MPS file.<br />
The header then goes on with null bytes until it is 2048 long.<br />

Usage:<br />
MPStoPMF videofile.mps<br />
MPStoPMF type:15 videofile.mps<br />
