.feature force_range
.debuginfo +
.setcpu "65c02"
.macpack longbranch
.list on

.feature org_per_seg
.segment "ZEROPAGE"
.zeropage
.org $0000

; Main code
.segment "CODE"
.org $8000

Reset:
    ; Immediate mode
    LDA #$AA
    LDA #$00

    LDX #$AA
    LDX #$00

    LDY #$AA
    LDY #$00

    ; Zeropage
    LDA #$AA
    STA $05
    LDA #$00
    LDA $05
    LDA #$00

    LDA #$00
    STA $05

    LDX #$AA
    STX $05
    LDX #$00
    LDX $05
    LDX #$00

    LDA #$00
    STA $05

    LDY #$AA
    STY $05
    LDY #$00
    LDY $05
    LDY #$00

    LDA #$00
    STA $05

    ; Zeropage,X

    ; TODO - Check page boundary cross

    LDA #$AA
    LDX #$03
    STA $05,X
    LDA #$00
    LDA $05,X
    LDA #$00

    LDA #$00
    STA $08
    LDX #$00

    LDY #$AA
    LDX #$03
    STY $05,X
    LDY #$00
    LDY $05,X
    LDY #$00

    LDA #$00
    STA $08
    LDX #$00

    ; Zeropage,Y
    LDX #$AA
    LDY #$03
    STX $05,Y
    LDX #$00
    LDX $05,Y
    LDX #$00

    LDA #$00
    STA $08
    LDX #$00

    ; Absolute
    LDA #$AA
    STA $5005
    LDA #$00
    LDA $5005
    LDA #$00

    LDA #$00
    STA $5005

    LDY #$AA
    STY $5005
    LDY #$00
    LDY $5005
    LDY #$00

    LDA #$00
    STA $5005

    LDX #$AA
    STX $5005
    LDX #$00
    LDX $5005
    LDX #$00

    LDA #$00
    STA $5005

    ; Absolute,X
    LDA #$AA
    LDX #$03
    STA $5005,X
    LDA #$00
    LDA $5005,X
    LDY $5005,X
    LDA #$00

    LDA #$00
    STA $5008
    LDX #$00
    LDY #$00

    LDA #$AA
    LDX #$03
    STA $4FFE,X
    LDA #$00
    LDA $4FFE,X
    LDY $4FFE,X
    LDA #$00
    LDX #$00

    LDA #$00
    STA $5001
    LDX #$00
    LDY #$00

    ; Absolute,Y
    LDA #$AA
    LDY #$03
    STA $5005,Y
    LDA #$00
    LDA $5005,Y
    LDX $5005,Y
    LDA #$00

    LDA #$00
    STA $5008
    LDY #$00
    LDY #$00

    LDA #$AA
    LDY #$03
    STA $4FFE,Y
    LDA #$00
    LDA $4FFE,Y
    LDX $4FFE,Y
    LDA #$00
    LDY #$00

    LDA #$00
    STA $5001
    LDY #$00
    LDY #$00

    ; Indirect,X
    LDA #$05
    STA $08
    LDA #$50
    STA $09
    LDX #$03
    LDA #$AA
    STA ($05,X)
    LDA #$00
    LDA ($05,X)

    LDA #$00
    STA $5005
    STA $08
    STA $09
    LDX #$00

    LDA #$05
    STA $FF
    LDA #$50
    STA $00
    LDX #$01
    LDA #$AA
    STA ($FE,X)
    LDA #$00
    LDA ($FE,X)

    LDA #$00
    STA $5005
    STA $FF
    STA $00
    LDX #$00
    LDY #$00

    LDA #$05
    STA $01
    LDA #$50
    STA $02
    LDX #$03
    LDA #$AA
    STA ($FE,X)
    LDA #$00
    LDA ($FE,X)

    LDA #$00
    STA $5005
    STA $01
    STA $02
    LDX #$00
    LDY #$00

    ; Indirect,Y
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDY #$03
    LDA #$AA
    STA ($05),Y
    LDA #$00
    LDA ($05),Y

    LDA #$00
    STA $5008
    STA $05
    STA $06
    LDY #$00

    LDA #$FE
    STA $FF
    LDA #$4F
    STA $00
    LDY #$05
    LDA #$AA
    STA ($FF),Y
    LDA #$00
    LDA ($FF),Y

    LDA #$00
    STA $5003
    STA $00
    STA $FF
    LDX #$00
    LDY #$00

    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDY #$05
    LDA #$AA
    STA ($05),Y
    LDA #$00
    LDA ($06),Y

    LDA #$00
    STA $5003
    STA $05
    STA $06
    LDX #$00
    LDY #$00

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
