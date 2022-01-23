.feature force_range
.feature org_per_seg
.feature pc_assignment
.debuginfo +
.setcpu "65c02"
.macpack longbranch
.list on

.segment "ZEROPAGE"
.zeropage
.org $0000

; Main code
.segment "CODE"
.org $8000

Reset:
    CLC
    CLD
    CLI
    CLV

    ; BNE
    LDA #$00
    CMP #$00
    BNE Branched1

NotBranched1:
    LDA #$AA
    JMP End1
Branched1:
    LDA #$55
End1:

    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$10
    BNE Branched2

NotBranched2:
    LDA #$AA
    JMP End2
Branched2:
    LDA #$55
End2:

    CLC
    CLD
    CLI
    CLV
    LDA #$10
    CMP #$00
    BNE Branched3

NotBranched3:
    LDA #$AA
    JMP End3
Branched3:
    LDA #$55
End3:

    ; BEQ
    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$00
    BEQ Branched4

NotBranched4:
    LDA #$AA
    JMP End4
Branched4:
    LDA #$55
End4:

    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$10
    BEQ Branched5

NotBranched5:
    LDA #$AA
    JMP End5
Branched5:
    LDA #$55
End5:

    CLC
    CLD
    CLI
    CLV
    LDA #$10
    CMP #$00
    BEQ Branched6

NotBranched6:
    LDA #$AA
    JMP End6
Branched6:
    LDA #$55
End6:

    ; BPL
    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$00
    BPL Branched7

NotBranched7:
    LDA #$AA
    JMP End7
Branched7:
    LDA #$55
End7:

    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$10
    BPL Branched8

NotBranched8:
    LDA #$AA
    JMP End8
Branched8:
    LDA #$55
End8:

    CLC
    CLD
    CLI
    CLV
    LDA #$10
    CMP #$00
    BPL Branched9

NotBranched9:
    LDA #$AA
    JMP End9
Branched9:
    LDA #$55
End9:

    ; BMI
    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$00
    BMI Branched10

NotBranched10:
    LDA #$AA
    JMP End10
Branched10:
    LDA #$55
End10:

    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$10
    BMI Branched11

NotBranched11:
    LDA #$AA
    JMP End11
Branched11:
    LDA #$55
End11:

    CLC
    CLD
    CLI
    CLV
    LDA #$10
    CMP #$00
    BMI Branched12

NotBranched12:
    LDA #$AA
    JMP End12
Branched12:
    LDA #$55
End12:

    ; BCC
    CLC
    CLD
    CLI
    CLV
    BCC Branched13

NotBranched13:
    LDA #$AA
    JMP End13
Branched13:
    LDA #$55
End13:

    SEC
    CLD
    CLI
    CLV
    BCC Branched14

NotBranched14:
    LDA #$AA
    JMP End14
Branched14:
    LDA #$55
End14:

    ; BCS
    CLC
    CLD
    CLI
    CLV
    BCS Branched15

NotBranched15:
    LDA #$AA
    JMP End15
Branched15:
    LDA #$55
End15:

    SEC
    CLD
    CLI
    CLV
    BCS Branched16

NotBranched16:
    LDA #$AA
    JMP End16
Branched16:
    LDA #$55
End16:

    ; BVC
    CLC
    CLD
    CLI
    CLV
    BVC Branched17

NotBranched17:
    LDA #$AA
    JMP End17
Branched17:
    LDA #$55
End17:

    CLC
    CLD
    CLI
    CLV
    LDA #$01
    CMP #$12
    BVC Branched18

NotBranched18:
    LDA #$AA
    JMP End18
Branched18:
    LDA #$55
End18:

    ; BVS
    CLC
    CLD
    CLI
    CLV
    BVS Branched19

NotBranched19:
    LDA #$AA
    JMP End19
Branched19:
    LDA #$55
End19:

    CLC
    CLD
    CLI
    CLV
    LDA #$01
    CMP #$12
    BVS Branched20

NotBranched20:
    LDA #$AA
    JMP End20
Branched20:
    LDA #$55
End20:

    JMP BoundaryCross21

.res $8FC0-*

    ; Page Boundary Crosses
BoundaryCross21:
    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$00
    BNE Branched21

NotBranched21:
    LDA #$AA
    JMP End21

.res $9000-*

Branched21:
    LDA #$55
End21:

    JMP BoundaryCross22

.res $9FC0-*

BoundaryCross22:
    CLC
    CLD
    CLI
    CLV
    LDA #$00
    CMP #$00
    BEQ Branched22

NotBranched22:
    LDA #$AA
    JMP End22

.res $A000-*

Branched22:
    LDA #$55
End22:

    BRK

IRQ:
    RTI

NMI:
    RTI

; Interupt vectors
.segment "VECTORS"
.org $FFFA
	.word	NMI
	.word	Reset
	.word	IRQ
